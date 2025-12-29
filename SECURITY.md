# Security Policy

## Supported Versions

We release patches for security vulnerabilities in the following versions:

| Version | Supported          |
| ------- | ------------------ |
| 1.x.x   | :white_check_mark: |
| < 1.0   | :x:                |

## Reporting a Vulnerability

We take security seriously. If you discover a security vulnerability, please follow these steps:

### 1. **DO NOT** create a public GitHub issue

Security vulnerabilities should be reported privately to protect users.

### 2. Report via GitHub Security Advisories

1. Go to the [Security tab](../../security/advisories)
2. Click "Report a vulnerability"
3. Fill in the details

### What to include:

- Description of the vulnerability
- Steps to reproduce
- Potential impact
- Suggested fix (if you have one)

### What happens next:

1. **Acknowledgment**: We'll confirm receipt within 48 hours
2. **Assessment**: We'll assess the severity within 7 days
3. **Fix**: We'll work on a fix (timeline depends on severity)
4. **Disclosure**: We'll coordinate disclosure with you
5. **Credit**: You'll be credited in the security advisory (if desired)

## Security Best Practices for Users

### GitHub Token

- **Never** commit your GitHub token to version control
- Create tokens with minimal required permissions
- Rotate tokens periodically
- Revoke tokens when no longer needed

### Configuration File

The `config.json` file contains:
- GitHub token (if configured)
- Repository URL
- Personal preferences

**Security tips**:
- Don't share your config.json file
- Don't upload it to cloud storage
- Be aware it's stored in plain text at `%APPDATA%\GitHubWallpaper\config.json`

### Network Security

The application makes HTTPS requests to:
- `api.github.com` - GitHub API
- Repository download URLs (for images)

**Security measures**:
- All connections use HTTPS
- No data is sent to third parties
- No telemetry or analytics

### Permissions

The application requires:
- **File system**: Read/write to Pictures folder and AppData
- **Registry**: Modify wallpaper settings (HKCU only)
- **Network**: Download images from GitHub

The application does **NOT** require:
- Administrator privileges
- Access to sensitive Windows areas
- Firewall exceptions

## Known Security Considerations

### 1. GitHub Token Storage

Tokens are stored in **plain text** in `config.json`. This is a known limitation.

**Mitigation**:
- Use tokens with minimal permissions (only `public_repo` for public repositories)
- Don't use personal access tokens with broad permissions
- Consider using fine-grained tokens (available in GitHub beta)

**Future improvement**: We plan to implement token encryption in a future release.

### 2. Registry Modifications

The application modifies Windows registry under `HKEY_CURRENT_USER`.

**Safety**:
- Only modifies user-specific settings
- Does not touch system-wide registry (HKLM)
- All modifications are reversible
- No security-sensitive registry keys are touched

### 3. Downloaded Content

Images are downloaded from GitHub repositories.

**Considerations**:
- Malicious images are theoretically possible
- Windows image codecs handle parsing
- Recommend only using trusted repositories

## Vulnerability Disclosure Policy

We follow **coordinated disclosure**:

1. Reporter notifies us privately
2. We confirm and work on a fix
3. We release the fix
4. We publish a security advisory
5. Public disclosure occurs

**Timeline**:
- Critical vulnerabilities: Fix within 7 days
- High severity: Fix within 30 days
- Medium/Low: Fix in next release

## Security Updates

Security updates are released as:
- Patch versions (e.g., 1.0.1) for minor fixes
- Minor versions (e.g., 1.1.0) for moderate fixes
- Major versions (e.g., 2.0.0) if breaking changes required

Check [Releases](../../releases) for security update notifications.

## Contact

For security-related questions: [your-email@example.com]

---

**Last updated**: December 2024
