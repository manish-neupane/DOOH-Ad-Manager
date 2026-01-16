# DOOH Angular Application

A modern, standalone Angular 21 application for managing Digital Out-of-Home (DOOH) advertisements.

## Features

- **Screens Management**: Create, edit, and manage digital display screens
- **Advertisements**: Upload and manage ad media files (video/images)
- **Campaigns**: Schedule ad campaigns with time slots and play order
- **Playlist Viewer**: View scheduled ads for any screen at any time
- **Modern UI**: Clean, responsive design with smooth animations

## Prerequisites

- Node.js (v18 or higher)
- Angular CLI 19+ (should already be installed)
- .NET 10 SDK (for the backend API)

## Setup Instructions

### 1. Install Dependencies

```bash
cd dooh-angular-app
npm install
```

### 2. Configure API Endpoint

The application is configured to connect to the backend at `http://localhost:5085/api`.

If your backend runs on a different port, update the `apiUrl` in:
- `src/environments/environment.ts`

### 3. Start the Backend API

Make sure your .NET backend is running:

```bash
cd /path/to/your/dotnet/project
dotnet run
```

The backend should be available at `http://localhost:5085`

### 4. Start the Angular Application

```bash
npm start
```

or

```bash
ng serve
```

The application will be available at `http://localhost:4200`

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── screens/         # Screen management
│   │   ├── ads/             # Advertisement management
│   │   ├── campaigns/       # Campaign scheduling
│   │   └── playlist/        # Playlist viewer
│   ├── models/              # TypeScript interfaces
│   ├── services/            # API services
│   ├── app.component.*      # Main app component
│   ├── app.routes.ts        # Route configuration
│   └── app.config.ts        # App configuration
├── environments/            # Environment configs
├── styles.css              # Global styles
└── index.html              # Main HTML file
```

## Key Technologies

- **Angular 21**: Standalone components architecture
- **TypeScript**: Type-safe development
- **RxJS**: Reactive programming
- **CSS Variables**: Themeable design system
- **Google Fonts**: Outfit and JetBrains Mono

## API Endpoints Used

### Screens
- GET `/api/screens` - List all screens
- POST `/api/screens` - Create screen
- PUT `/api/screens/{id}` - Update screen
- PATCH `/api/screens/{id}/status` - Update status
- DELETE `/api/screens/{id}` - Delete screen

### Ads
- GET `/api/ads` - List all ads
- POST `/api/ads/upload` - Upload ad with media
- DELETE `/api/ads/{id}` - Delete ad

### Campaigns
- GET `/api/campaigns` - List all campaigns
- POST `/api/campaigns` - Create campaign
- PUT `/api/campaigns/{id}` - Update campaign
- DELETE `/api/campaigns/{id}` - Delete campaign

### Playlist
- GET `/api/screens/{id}/playlist?at=<timestamp>` - Get playlist

### Proof of Play
- POST `/api/proofofplay` - Record playback

## Development

### Building for Production

```bash
ng build --configuration production
```

The build artifacts will be stored in the `dist/` directory.

### Running Tests

```bash
ng test
```

## Features in Detail

### Screens
- Create and manage display screens
- Set location, resolution, and orientation
- Control screen status (Active/Inactive/Maintenance)

### Advertisements
- Upload video and image files
- Set ad duration
- Preview media content
- Track file size and type

### Campaigns
- Schedule ads to specific screens
- Set time ranges
- Define play order for multiple ads
- Prevent scheduling conflicts

### Playlist Viewer
- Select any screen
- Choose specific date/time
- View all ads scheduled at that moment
- See total playlist duration

## Troubleshooting

### API Connection Issues
- Verify backend is running on port 5085
- Check CORS settings in backend
- Ensure firewall allows connections

### Build Errors
- Delete `node_modules` and reinstall: `rm -rf node_modules && npm install`
- Clear Angular cache: `ng cache clean`

## License

This project is created for DOOH advertisement management.
