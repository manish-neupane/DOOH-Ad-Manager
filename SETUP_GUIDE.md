# Quick Setup Guide - DOOH Ad Manager

## ⚡ 5-Minute Setup

### Step 1: Prerequisites Check
```bash
# Check .NET version
dotnet --version
# Should show: 8.0.x or higher

# Check Node.js version
node --version
# Should show: v18.x or higher

# Check PostgreSQL
psql --version
# Should show: psql (PostgreSQL) 14.x or higher
```

### Step 2: Database Setup
```bash
# Create database
createdb -U postgres dooh_db

# Or using psql
psql -U postgres
CREATE DATABASE dooh_db;
\q
```

### Step 3: Backend Setup
```bash
# Navigate to backend
cd DoohAdManager-Fixed-V3

# Update connection string in appsettings.json
# Change password to your PostgreSQL password

# Restore and run
dotnet restore
dotnet run
```

✅ Backend running at: `http://localhost:5085`  
✅ Swagger at: `http://localhost:5085/swagger`

### Step 4: Frontend Setup
```bash
# Open new terminal
cd angular-frontend

# Install dependencies
npm install

# Update API URL in src/environments/environment.ts if needed

# Run frontend
ng serve
```

✅ Frontend running at: `http://localhost:4200`

### Step 5: Verify Setup

1. Open browser: `http://localhost:4200`
2. You should see the DOOH Ad Manager interface
3. Try creating a screen to verify everything works

---

## 🐳 Docker Setup (Alternative)

### Using Docker Compose

**1. Create `docker-compose.yml`:**
```yaml
version: '3.8'

services:
  postgres:
    image: postgres:14
    environment:
      POSTGRES_DB: dooh_db
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  backend:
    build: ./DoohAdManager-Fixed-V3
    ports:
      - "5085:8080"
    environment:
      ConnectionStrings__PostgresConnection: "Host=postgres;Database=dooh_db;Username=postgres;Password=postgres"
    depends_on:
      - postgres

  frontend:
    build: ./angular-frontend
    ports:
      - "4200:80"
    depends_on:
      - backend

volumes:
  postgres_data:
```

**2. Run:**
```bash
docker-compose up -d
```

---

## 🔧 Troubleshooting

### Backend Won't Start

**Problem:** Port 5085 already in use
```bash
# Find process using port
lsof -i :5085

# Kill it
kill -9 <PID>
```

**Problem:** Database connection fails
```bash
# Check PostgreSQL is running
sudo systemctl status postgresql

# Start if needed
sudo systemctl start postgresql

# Test connection
psql -U postgres -d dooh_db
```

### Frontend Won't Start

**Problem:** Port 4200 already in use
```bash
# Run on different port
ng serve --port 4300
```

**Problem:** npm install fails
```bash
# Clear cache
npm cache clean --force
rm -rf node_modules package-lock.json
npm install
```

### CORS Errors

**Problem:** Browser shows CORS error

**Solution:** Check backend CORS configuration in `Program.cs`:
```csharp
app.UseCors("AllowAll");
```

### File Upload Fails

**Problem:** Cannot upload media files

**Solution 1:** Check uploads directory exists
```bash
cd DoohAdManager-Fixed-V3
mkdir -p wwwroot/uploads
chmod 755 wwwroot/uploads
```

**Solution 2:** Check file size limits in `appsettings.json`

---

## 📱 First Time Usage

### 1. Create Your First Screen
- Click "Screens" in sidebar
- Click "+ New Screen"
- Fill in:
  - Name: "Main Entrance Display"
  - Location: "Building A, Ground Floor"
  - Status: "Active"
  - Resolution: "1920x1080"
- Click "Save"

### 2. Upload Your First Ad
- Click "Advertisements" in sidebar
- Click "+ Upload Ad"
- Fill in:
  - Name: "Welcome Ad"
  - Duration: 30 seconds
  - Choose a video or image file
- Click "Upload"

### 3. Create Your First Campaign
- Click "Campaigns" in sidebar
- Click "+ New Campaign"
- Fill in:
  - Name: "January Promotion"
  - Select screens: Check "Main Entrance Display"
  - Select ads: Check "Welcome Ad"
  - Start Time: Today's date, 00:00
  - End Time: End of month, 23:59
  - Play Order: 1
- Click "Save"

### 4. View Playlist
- Click "Playlist Viewer" in sidebar
- Select a screen to see its current playlist
- Or use API: `http://localhost:5085/api/screens/1/playlist`

---

## 🧪 API Testing

### Using Swagger
1. Open: `http://localhost:5085/swagger`
2. Expand any endpoint
3. Click "Try it out"
4. Fill in parameters
5. Click "Execute"
6. See response below

### Using cURL

**Create Screen:**
```bash
curl -X POST "http://localhost:5085/api/screens" \
  -H "Content-Type: application/json" \
  -d '{"name":"Test","location":"Test Location","status":"Active"}'
```

**Get Screens:**
```bash
curl "http://localhost:5085/api/screens"
```

**Upload Ad:**
```bash
curl -X POST "http://localhost:5085/api/ads/upload" \
  -F "file=@video.mp4" \
  -F "title=Test Ad" \
  -F "durationSeconds=30"
```

---

## 📊 Database Management

### View Database
```bash
# Connect to database
psql -U postgres -d dooh_db

# List tables
\dt

# View screens
SELECT * FROM "Screens";

# View campaigns
SELECT * FROM "Campaigns";

# Exit
\q
```

### Backup Database
```bash
pg_dump -U postgres dooh_db > dooh_backup.sql
```

### Restore Database
```bash
psql -U postgres dooh_db < dooh_backup.sql
```

### Reset Database
```bash
# Drop and recreate
dropdb -U postgres dooh_db
createdb -U postgres dooh_db

# Application will recreate schema on next run
```

---

## 🚀 Production Deployment Checklist

### Before Deploying:

- [ ] Update `appsettings.Production.json` with production database
- [ ] Enable HTTPS only
- [ ] Implement authentication/authorization
- [ ] Set up proper CORS policy
- [ ] Configure file upload limits
- [ ] Set up logging (Serilog, Application Insights)
- [ ] Implement database migrations (not EnsureCreated)
- [ ] Set up CDN for media files
- [ ] Configure error handling
- [ ] Set up monitoring and alerts
- [ ] Implement rate limiting
- [ ] Set up automated backups
- [ ] Configure load balancer (if scaling)
- [ ] Set up SSL certificates
- [ ] Review security headers
- [ ] Test error scenarios
- [ ] Document API changes
- [ ] Set up CI/CD pipeline

---

## 📞 Getting Help

### Check These First:
1. **Swagger Documentation**: `http://localhost:5085/swagger`
2. **README.md**: Main documentation
3. **API_DOCUMENTATION.md**: Detailed API reference
4. **Console Logs**: Check terminal for error messages
5. **Browser Console**: F12 → Console tab for frontend errors

### Common Issues:

**"Cannot connect to API"**
- Check backend is running on port 5085
- Check environment.ts has correct API URL
- Check CORS configuration

**"Database error"**
- Verify PostgreSQL is running
- Check connection string in appsettings.json
- Verify database exists

**"File upload fails"**
- Check wwwroot/uploads directory exists
- Check file permissions
- Check file size (default limit: 50MB)

**"Campaigns not showing in playlist"**
- Verify campaign dates include current time
- Check screens are assigned to campaign
- Check ads are assigned to campaign

---

## 🎓 Learning Resources

### Understanding the Codebase:

**Backend:**
- `Controllers/` - API endpoints
- `Models/` - Database entities
- `Data/ApplicationDbContext.cs` - EF Core configuration
- `Program.cs` - Application startup

**Frontend:**
- `src/app/components/` - UI components
- `src/app/services/` - API communication
- `src/app/models/` - TypeScript interfaces
- `src/app/app.routes.ts` - Routing configuration

### Key Concepts:

1. **Campaign Time Logic**: Campaigns are active when current time falls between startTime and endTime
2. **Play Order**: Lower numbers play first (Campaign playOrder, then Ad playOrder)
3. **Many-to-Many**: Campaigns link to multiple screens and multiple ads through junction tables
4. **Proof of Play**: Records are created when ads actually play on screens
5. **UTC Time**: All timestamps stored and processed in UTC

---

## ✅ Verification Checklist

After setup, verify everything works:

### Backend:
- [ ] API responds at http://localhost:5085/api/screens
- [ ] Swagger UI loads at http://localhost:5085/swagger
- [ ] Database connection successful (check logs)
- [ ] Can create screens via API
- [ ] File uploads work (try uploading test video)

### Frontend:
- [ ] UI loads at http://localhost:4200
- [ ] Can navigate between pages
- [ ] Can create screens
- [ ] Can upload ads
- [ ] Can create campaigns
- [ ] Playlist viewer shows data
- [ ] Proof of Play shows records

### Integration:
- [ ] Frontend can fetch screens from backend
- [ ] File uploads work from UI
- [ ] Campaign creation links screens and ads
- [ ] Playlist API returns correct data
- [ ] Status updates work

---

## 🎯 Next Steps

1. **Customize**: Update branding, colors, logos
2. **Add Features**: Implement additional requirements
3. **Secure**: Add authentication and authorization
4. **Scale**: Set up load balancing and caching
5. **Monitor**: Implement logging and metrics
6. **Test**: Write unit and integration tests
7. **Document**: Add inline code comments
8. **Deploy**: Move to production environment

---

**Setup Time:** ~5-10 minutes  
**Difficulty:** Easy  
**Support:** Check README.md and API documentation

