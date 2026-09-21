using Microsoft.Extensions.DependencyInjection;

using StreamMaster.Domain.Configuration;
using StreamMaster.Domain.Repository;

namespace StreamMaster.Domain.Services;

public class ProfileService(IOptionsMonitor<Setting> intSettings, IServiceProvider serviceProvider, IOptionsMonitor<OutputProfileDict> intOutProfileSettings, IOptionsMonitor<CommandProfileDict> intCommandProfileSettings
    ) : IProfileService
{
    public List<CommandProfileDto> GetCommandProfiles()
    {
        return intCommandProfileSettings.CurrentValue.GetProfilesDto();
    }

    public CommandProfileDto GetCommandProfile(string? CommandProfileName = null)
    {
        Setting settings = intSettings.CurrentValue;

        return !string.IsNullOrEmpty(CommandProfileName) && CommandProfileName != settings.DefaultCommandProfileName
            ? intCommandProfileSettings.CurrentValue.GetProfileDto(CommandProfileName)
            : intCommandProfileSettings.CurrentValue.GetDefaultProfileDto(settings.DefaultCommandProfileName);
    }

    public OutputProfileDto GetOutputProfile(string? OutputProfileName = null)
    {
        Setting settings = intSettings.CurrentValue;

        return !string.IsNullOrEmpty(OutputProfileName) && OutputProfileName != settings.DefaultOutputProfileName
           ? intOutProfileSettings.CurrentValue.GetProfileDto(OutputProfileName)
           : intOutProfileSettings.CurrentValue.GetDefaultProfileDto(settings.DefaultOutputProfileName);
    }

    public CommandProfileDto GetM3U8OutputProfile(string id, CommandProfileDto? preferredCommandProfile = null)
    {
        Setting settings = intSettings.CurrentValue;

        // Prefer an explicit channel/stream command profile over M3U8 file/settings overrides
        if (IsExplicitCommandProfile(preferredCommandProfile, settings))
        {
            return preferredCommandProfile!;
        }

        using IServiceScope scope = serviceProvider.CreateScope();
        IRepositoryWrapper repositoryWrapper = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();
        SMStream? smStream = repositoryWrapper.SMStream.GetSMStreamById(id);

        if (IsExplicitCommandProfileName(smStream?.CommandProfileName, settings)
            && intCommandProfileSettings.CurrentValue.HasProfile(smStream!.CommandProfileName!))
        {
            return intCommandProfileSettings.CurrentValue.GetProfileDto(smStream.CommandProfileName!);
        }

        // Prefer SMChannel.CommandProfileName when not Default (matched via BaseStreamID)
        SMChannel? smChannel = repositoryWrapper.SMChannel.GetQuery()
            .Where(c => c.BaseStreamID == id)
            .AsEnumerable()
            .FirstOrDefault(c => IsExplicitCommandProfileName(c.CommandProfileName, settings));
        if (smChannel != null && intCommandProfileSettings.CurrentValue.HasProfile(smChannel.CommandProfileName))
        {
            return intCommandProfileSettings.CurrentValue.GetProfileDto(smChannel.CommandProfileName);
        }

        if (smStream?.M3UFileId > 0)
        {
            M3UFile? m3uFile = repositoryWrapper.M3UFile.GetQuery().FirstOrDefault(m => m.Id == smStream.M3UFileId);
            if (m3uFile != null && !string.IsNullOrEmpty(m3uFile.M3U8OutPutProfile) && intCommandProfileSettings.CurrentValue.HasProfile(m3uFile.M3U8OutPutProfile))
            {
                return intCommandProfileSettings.CurrentValue.GetProfileDto(m3uFile.M3U8OutPutProfile);
            }
        }

        if (!string.IsNullOrEmpty(settings.M3U8OutPutProfile))
        {
            if (intCommandProfileSettings.CurrentValue.HasProfile(settings.M3U8OutPutProfile))
            {
                CommandProfileDto ret = intCommandProfileSettings.CurrentValue.GetProfileDto(settings.M3U8OutPutProfile);
                return ret;
            }
        }
        return intCommandProfileSettings.CurrentValue.GetProfileDto("SMFFMPEG");
    }

    private static bool IsExplicitCommandProfile(CommandProfileDto? profile, Setting settings)
    {
        return profile != null && IsExplicitCommandProfileName(profile.ProfileName, settings);
    }

    private static bool IsExplicitCommandProfileName(string? profileName, Setting settings)
    {
        return !string.IsNullOrEmpty(profileName)
            && !string.Equals(profileName, "Default", StringComparison.InvariantCultureIgnoreCase)
            && !string.Equals(profileName, settings.DefaultCommandProfileName, StringComparison.InvariantCultureIgnoreCase);
    }
}