# Cloudflare Dynamic DNS
This project is NOT related or maintained by Cloudflare.

## Description
The software will check the IP address of the given DNS records in Cloudflare and will match it against the current public IP address. The software will update the IP address of the DNS records in Cloudflare through the API when the IP addresses differ from the current public IP address.

## Installation
Pull the docker image and mount a volume from the host for persistent storage:
```
docker pull oezie/cfdyndns:latest
docker run -d --name cfdyndns -v /host-data/:/data/ --restart unless-stopped oezie/cfdyndns:latest
```
The software will add an appsettings.json file in the /data folder. Please update the settings in this file according to your needs.

## Configuration
The appsettings.json consists of multiple sections explained below.

### Cloudflare
<table>
<tr><th>Setting</th><th>Description</th><th>Default value</th></tr>
<tr>
	<td><strong>Url</strong></td>
	<td>The url of the Cloudflare API.</td>
	<td>https://api.cloudflare.com/client/v4/</td>
</tr>
<tr>
	<td><strong>Token</strong></td>
	<td>Your Cloudflare API token. Request it via <a href="https://dash.cloudflare.com/profile/api-tokens">https://dash.cloudflare.com/profile/api-tokens</a>. Please make sure that the token has the DNS edit rights for the zone of your DNS records.</td>
	<td></td>
</tr>
<tr>
	<td><strong>Records</strong></td>
	<td>An array of the DNS records that you want to change in case of an IP address change. <u>Only A-records are currently supported!</u></td>
	<td></td>
</tr>
</table>

### IP API
<table>
<tr><th>Setting</th><th>Description</th><th>Default value</th></tr>
<tr>
	<td><strong>UrlIpApi</strong></td>
	<td>A URL that returns your IP address. You can use any site as long as the URL returns the IP address in plain text. The default URL is adviced.</td>
	<td><a href="https://myip.moes.network/">https://myip.moes.network/</a></td>
</tr>
<tr>
	<td><strong>PollFreq</strong></td>
	<td>The frequency in minutes in which the program should check for a changed IP address.</td>
	<td>60</td>
</tr>
</table>

### Telegram (optional)
The program can notify you in case of an IP address change via Telegram. Leave these settings empty if you don't want to use this feature.
<table>
<tr><th>Setting</th><th>Description</th><th>Default value</th></tr>
<tr>
	<td><strong>ApiToken</strong></td>
	<td>Your Telegram bot token.</td>
	<td></td>
</tr>
<tr>
	<td><strong>ChatId</strong></td>
	<td>An integer with the Telegram Chat ID where you want to receive your notifications.</td>
	<td></td>
</tr>
</table>
