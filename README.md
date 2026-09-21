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

Docker Hub alternative: `hhankj2u/streammaster:latest`

- [GHCR packages](https://github.com/hhankj2u/StreamMaster/pkgs/container/streammaster)
- [Docker Hub](https://hub.docker.com/r/hhankj2u/streammaster)
- [Issues](https://github.com/hhankj2u/StreamMaster/issues) · [Discussions](https://github.com/hhankj2u/StreamMaster/discussions) · [Contributing](.github/CONTRIBUTING.md)

Public channel lists: [iptv-org](https://github.com/iptv-org/iptv) · Logos: [tv-logos](https://github.com/tv-logo/tv-logos)

## Credits

Based on [carlreid/StreamMaster](https://github.com/carlreid/StreamMaster).

## License

[MIT](LICENSE)
