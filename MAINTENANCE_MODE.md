# Maintenance Mode Feature

## Overview

This solution now includes a maintenance mode feature that allows you to display a maintenance page to users while performing system upgrades or maintenance.
## How It Works

The `MaintenanceModeMiddleware` checks the configuration on every request. When maintenance mode is enabled:
- All requests (except health checks) receive a 503 Service Unavailable status
- Users are shown the maintenance page from `wwwroot/maintenance.html`
- The response includes a `Retry-After` header suggesting users try again in 1 hour

## Configuration

Maintenance mode is controlled through the `appsettings.json` configuration file:

```json
{
  "MaintenanceMode": {
    "Enabled": false
  }
}
```

### Settings

- **Enabled**: Set to `true` to enable maintenance mode, `false` to disable

## Health Checks

The `/dotnet-health` endpoint is always accessible, even during maintenance mode. This ensures your monitoring systems can still check if the application is running.

## Customizing the Maintenance Page

Edit `wwwroot/maintenance.html` to customize the appearance and message shown to users during maintenance.
