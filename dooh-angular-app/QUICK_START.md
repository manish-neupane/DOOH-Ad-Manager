# DOOH Angular App - Quick Start Guide

## What You Have

A complete, working Angular 21 standalone application that connects to your .NET DOOH API.

## File Structure Overview

```
dooh-angular-app/
│
├── src/
│   ├── app/
│   │   ├── components/           # UI Components
│   │   │   ├── screens/         # Manage screens
│   │   │   ├── ads/             # Manage ads
│   │   │   ├── campaigns/       # Schedule campaigns
│   │   │   └── playlist/        # View playlists
│   │   │
│   │   ├── models/              # TypeScript interfaces
│   │   │   ├── screen.model.ts
│   │   │   ├── ad.model.ts
│   │   │   └── campaign.model.ts
│   │   │
│   │   ├── services/            # API services
│   │   │   ├── screen.service.ts
│   │   │   ├── ad.service.ts
│   │   │   └── campaign.service.ts
│   │   │
│   │   ├── app.component.*      # Main layout
│   │   ├── app.routes.ts        # Routing
│   │   └── app.config.ts        # Configuration
│   │
│   ├── environments/            # Environment configs
│   ├── styles.css              # Global styles
│   └── index.html
│
├── package.json                # Dependencies
├── angular.json               # Angular config
├── tsconfig.json              # TypeScript config
└── README.md                  # Documentation

```

## Getting Started (3 Steps)

### Step 1: Install Dependencies

Open terminal in the `dooh-angular-app` folder and run:

```bash
npm install
```

### Step 2: Start Your Backend

Make sure your .NET API is running:

```bash
dotnet run
```

It should be at: `http://localhost:5085`

### Step 3: Start Angular App

```bash
npm start
```

Open browser at: `http://localhost:4200`

## Understanding the Components

### 1. Screens Component (`src/app/components/screens/`)
- **What it does**: Manages digital display screens
- **Files**:
  - `screens.component.ts` - Logic
  - `screens.component.html` - Template
  - `screens.component.css` - Styles

### 2. Ads Component (`src/app/components/ads/`)
- **What it does**: Upload and manage advertisements
- **Special**: Handles file uploads with FormData

### 3. Campaigns Component (`src/app/components/campaigns/`)
- **What it does**: Schedule ads to screens
- **Features**: Prevents overlapping campaigns

### 4. Playlist Component (`src/app/components/playlist/`)
- **What it does**: View scheduled ads at specific times
- **API call**: `GET /api/screens/{id}/playlist?at=<timestamp>`

## Key Services

All services are in `src/app/services/`:

### ScreenService
```typescript
getScreens()                        // Get all screens
createScreen(screen)                // Create new screen
updateScreen(id, screen)            // Update screen
updateScreenStatus(id, status)      // Change status
deleteScreen(id)                    // Delete screen
```

### AdService
```typescript
getAds()                           // Get all ads
uploadAd(name, duration, file)     // Upload new ad
deleteAd(id)                       // Delete ad
```

### CampaignService
```typescript
getCampaigns()                     // Get all campaigns
createCampaign(campaign)           // Create campaign
updateCampaign(id, campaign)       // Update campaign
deleteCampaign(id)                 // Delete campaign
getPlaylist(screenId, at?)         // Get playlist
recordProofOfPlay(proof)           // Record playback
```

## Modifying the App

### Change API URL

Edit `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://your-api-url:port/api'  // Change this
};
```

### Add New Field to Screen

1. Update model in `src/app/models/screen.model.ts`
2. Update form in `src/app/components/screens/screens.component.html`
3. Update component logic in `screens.component.ts`

### Change Theme Colors

Edit CSS variables in `src/styles.css`:

```css
:root {
  --primary-color: #0ea5e9;     /* Change primary color */
  --accent-color: #06b6d4;      /* Change accent color */
  /* ... more colors ... */
}
```

## Common Tasks

### Adding a New Page

1. Create component:
```bash
ng generate component components/your-component --standalone
```

2. Add route in `src/app/app.routes.ts`:
```typescript
{ path: 'your-path', component: YourComponent }
```

3. Add navigation link in `src/app/app.component.html`

### Creating a New Service

1. Generate service:
```bash
ng generate service services/your-service
```

2. Add to component:
```typescript
constructor(private yourService: YourService) {}
```

## Troubleshooting

### "Cannot find module" errors
```bash
rm -rf node_modules package-lock.json
npm install
```

### API not connecting
1. Check backend is running: `http://localhost:5085/api/screens`
2. Check CORS is enabled in backend
3. Verify API URL in environment.ts

### Port 4200 already in use
```bash
ng serve --port 4300
```

## Building for Production

```bash
ng build --configuration production
```

Output will be in `dist/dooh-angular-app/browser/`

Deploy these files to any web server.

## Tips for Success

1. **Standalone Architecture**: All components are standalone (no NgModule needed)
2. **Type Safety**: Use TypeScript interfaces for all data
3. **Reactive**: Services use RxJS Observables
4. **Responsive**: Works on mobile and desktop
5. **Modern**: Uses Angular 21 best practices

## Next Steps

1. ✅ Install and run the app
2. ✅ Test all CRUD operations
3. ✅ Customize colors and branding
4. ✅ Add your own features
5. ✅ Deploy to production

## Need Help?

- Check the browser console for errors (F12)
- Check the network tab to see API calls
- Review the README.md for more details
- All components follow the same pattern, so you can learn by example

Happy coding! 🚀
