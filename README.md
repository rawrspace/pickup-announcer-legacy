# Pickup Announcer

- Announce student pickups to all Teachers who are connected to the SignalR hub.
- Define Grade Levels and customize the announcement colors.
- Configure the number of cones available for pickup.

## Setup

The MSSQL Database Project needs to be published before the application can run. Each instance of the application will require a separate database instance.

## Running

After cloning the solution open in Visual Studio/VS Code and package the application for deployment based on your target environment.

Make sure to modify the appsettings.json file with the ConnectionString set for DefaultConnection so that the container can access your MSSQL instance.

## Configuration

Once the database has been deployed you should update the Config.Site table to set new values for AdminUser and AdminPass. Be sure to communicate these values to the Principal of the school who will use the application.

*Note: This admin account is only meant to prevent accidental access to the admin panel. Therefore the password is stored in plain text. Please do NOT reuse a password that could be comprimised.*

## Usage

Teachers will use the application by navigating to the main page and simply leaving the app open. On initial connect all of the announcements for the day will automatically download and then an open SignalR connection will be used to send additional announcements.

The announcer working car rider pickup will use the Announcement page to type in the registration number and the cone where the car will be location. Once submitted the registration ID will be checked and if valid an announcement will be sent.

The administration can access the Admin page by logging in with the credentials set during configuration. In the admin panel they can download a copy of the database records, modify, and reupload the data to alter the student records. Admin can also set the number of cones they have on campus and configure the color of announcements for each grade level that is setup.

## Contributing
Check out [CONTRIBUTING.md](CONTRIBUTING.md) for more info.

## License
[![License](https://img.shields.io/github/license/rawrspace/pickup-announcer-legacy.svg)](https://github.com/rawrspace/pickup-announcer-legacy/blob/master/LICENSE)