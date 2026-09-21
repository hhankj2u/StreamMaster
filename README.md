<p align="center" width="100%">
    <img src="https://raw.githubusercontent.com/hhankj2u/StreamMaster/refs/heads/main/src/StreamMaster.WebUI/public/images/streammaster_logo.png" alt="StreamMaster Logo"/>
    <H1 align="center" width="100%">StreamMaster</H1>
</p>

> IPTV management for organizing and streaming public broadcast content through Plex DVR, Emby, and Jellyfin Live TV.

## Quick Start

```yaml
services:
  streammaster:
    image: ghcr.io/hhankj2u/streammaster:latest
    container_name: streammaster
    ports:
      - 7095:7095
    restart: unless-stopped
    volumes:
      - ~/streammaster:/config
      - ~/streammaster/tv-logos:/config/tv-logos
```

- [GHCR packages](https://github.com/hhankj2u/StreamMaster/pkgs/container/streammaster)
- [Issues](https://github.com/hhankj2u/StreamMaster/issues) · [Discussions](https://github.com/hhankj2u/StreamMaster/discussions) · [Contributing](.github/CONTRIBUTING.md)

Public channel lists: [iptv-org](https://github.com/iptv-org/iptv) · Logos: [tv-logos](https://github.com/tv-logo/tv-logos)

## What's New in This Fork

**Per-channel command profiles for HLS/M3U8** — If a channel’s Profile Name is set to something other than `Default`, that profile is used for playback (including `.m3u8` / HLS streams). The M3U file `M3U8OutPutProfile` only applies when the channel is still on `Default`.

Priority: channel profile → stream group profile → M3U8 file / settings fallback.

## Credits

Based on [carlreid/StreamMaster](https://github.com/carlreid/StreamMaster).

## License

[MIT](LICENSE)
