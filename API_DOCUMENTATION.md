# DOOH Ad Manager - API Documentation

## Base Information

**Base URL:** `http://localhost:5085/api`  
**Content-Type:** `application/json`  
**Authentication:** None  
**Swagger UI:** `http://localhost:5085/swagger`

---

## API Endpoints Overview

| Resource | Method | Endpoint | Description |
|----------|--------|----------|-------------|
| **Screens** | GET | `/api/screens` | List all screens |
| | GET | `/api/screens/{id}` | Get screen by ID |
| | POST | `/api/screens` | Create new screen |
| | PUT | `/api/screens/{id}` | Update screen |
| | DELETE | `/api/screens/{id}` | Delete screen |
| **Ads** | GET | `/api/ads` | List all ads |
| | POST | `/api/ads/upload` | Upload new ad |
| | DELETE | `/api/ads/{id}` | Delete ad |
| **Campaigns** | GET | `/api/campaigns` | List all campaigns |
| | POST | `/api/campaigns` | Create campaign |
| | PUT | `/api/campaigns/{id}` | Update campaign |
| | DELETE | `/api/campaigns/{id}` | Delete campaign |
| **Playlist** | GET | `/api/screens/{id}/playlist` | Get screen playlist |
| **Proof of Play** | GET | `/api/proofofplay` | Get proof records |
| | POST | `/api/proofofplay` | Record playback |

---

## 🖥️ Screens API

### List All Screens

**GET** `/api/screens`

Returns a list of all digital screens in the system.

**Request:**
```http
GET /api/screens HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**
```json
[
  {
    "id": 1,
    "name": "Mall Entrance Display",
    "location": "Ground Floor, Main Entrance",
    "status": "Active",
    "resolution": "1920x1080",
    "orientation": "Landscape"
  },
  {
    "id": 2,
    "name": "Food Court Screen",
    "location": "3rd Floor, Food Court",
    "status": "Active",
    "resolution": "3840x2160",
    "orientation": "Portrait"
  }
]
```

---

### Get Screen by ID

**GET** `/api/screens/{id}`

Get details of a specific screen.

**Parameters:**
- `id` (path, required): Screen ID

**Request:**
```http
GET /api/screens/1 HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**
```json
{
  "id": 1,
  "name": "Mall Entrance Display",
  "location": "Ground Floor, Main Entrance",
  "status": "Active",
  "resolution": "1920x1080",
  "orientation": "Landscape"
}
```

**Response: 404 Not Found**
```json
{
  "error": "Screen not found"
}
```

---

### Create Screen

**POST** `/api/screens`

Create a new digital screen.

**Request Body:**
```json
{
  "name": "New Screen Display",
  "location": "5th Floor, East Wing",
  "status": "Active",
  "resolution": "1920x1080",
  "orientation": "Landscape"
}
```

**Field Descriptions:**
- `name` (required): Screen display name
- `location` (required): Physical location description
- `status` (optional): "Active", "Offline", or "Maintenance" (default: "Active")
- `resolution` (optional): Display resolution (e.g., "1920x1080")
- `orientation` (optional): "Landscape" or "Portrait"

**Response: 201 Created**
```json
{
  "id": 3,
  "name": "New Screen Display",
  "location": "5th Floor, East Wing",
  "status": "Active",
  "resolution": "1920x1080",
  "orientation": "Landscape"
}
```

**Response: 400 Bad Request**
```json
{
  "errors": {
    "Name": ["The Name field is required."],
    "Location": ["The Location field is required."]
  }
}
```

---

### Update Screen

**PUT** `/api/screens/{id}`

Update an existing screen.

**Parameters:**
- `id` (path, required): Screen ID to update

**Request Body:**
```json
{
  "name": "Updated Screen Name",
  "location": "Updated Location",
  "status": "Maintenance",
  "resolution": "3840x2160",
  "orientation": "Portrait"
}
```

**Response: 200 OK**
```json
{
  "id": 1,
  "name": "Updated Screen Name",
  "location": "Updated Location",
  "status": "Maintenance",
  "resolution": "3840x2160",
  "orientation": "Portrait"
}
```

**Response: 404 Not Found**

---

### Delete Screen

**DELETE** `/api/screens/{id}`

Delete a screen.

**Parameters:**
- `id` (path, required): Screen ID to delete

**Request:**
```http
DELETE /api/screens/3 HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**

**Response: 404 Not Found**

---

## 📺 Advertisements API

### List All Ads

**GET** `/api/ads`

Get all advertisements.

**Request:**
```http
GET /api/ads HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**
```json
[
  {
    "id": 1,
    "title": "Summer Sale Promo",
    "mediaUrl": "/uploads/ad_20260115_103045.mp4",
    "durationSeconds": 30,
    "mediaType": "video/mp4",
    "fileSize": 15728640,
    "createdAt": "2026-01-15T10:30:45Z"
  },
  {
    "id": 2,
    "title": "Product Launch",
    "mediaUrl": "/uploads/ad_20260115_110512.jpg",
    "durationSeconds": 15,
    "mediaType": "image/jpeg",
    "fileSize": 2097152,
    "createdAt": "2026-01-15T11:05:12Z"
  }
]
```

---

### Upload Ad

**POST** `/api/ads/upload`

Upload a new advertisement media file.

**Content-Type:** `multipart/form-data`

**Form Fields:**
- `file` (required): Media file (image or video)
- `title` (required): Advertisement title
- `durationSeconds` (required): Playback duration in seconds

**Request Example (cURL):**
```bash
curl -X POST "http://localhost:5085/api/ads/upload" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/path/to/video.mp4" \
  -F "title=Summer Sale Promo" \
  -F "durationSeconds=30"
```

**Request Example (JavaScript/Fetch):**
```javascript
const formData = new FormData();
formData.append('file', fileInput.files[0]);
formData.append('title', 'Summer Sale Promo');
formData.append('durationSeconds', '30');

fetch('http://localhost:5085/api/ads/upload', {
  method: 'POST',
  body: formData
})
.then(response => response.json())
.then(data => console.log(data));
```

**Response: 201 Created**
```json
{
  "id": 1,
  "title": "Summer Sale Promo",
  "mediaUrl": "/uploads/ad_20260115_103045.mp4",
  "durationSeconds": 30,
  "mediaType": "video/mp4",
  "fileSize": 15728640,
  "createdAt": "2026-01-15T10:30:45Z"
}
```

**Response: 400 Bad Request**
```json
{
  "error": "File is required"
}
```

**Supported File Types:**
- **Images:** JPEG, PNG, GIF, WEBP
- **Videos:** MP4, WEBM, AVI, MOV

---

### Delete Ad

**DELETE** `/api/ads/{id}`

Delete an advertisement.

**Parameters:**
- `id` (path, required): Ad ID to delete

**Request:**
```http
DELETE /api/ads/1 HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**

**Response: 404 Not Found**

**Note:** Deleting an ad does not remove associated media files from disk.

---

## 📅 Campaigns API

### List All Campaigns

**GET** `/api/campaigns`

Get all campaigns with associated screens and ads.

**Request:**
```http
GET /api/campaigns HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**
```json
[
  {
    "id": 1,
    "name": "Summer Campaign 2026",
    "startTime": "2026-06-01T00:00:00Z",
    "endTime": "2026-08-31T23:59:59Z",
    "playOrder": 1,
    "campaignScreens": [
      {
        "screenId": 1,
        "screen": {
          "id": 1,
          "name": "Mall Entrance Display",
          "location": "Ground Floor"
        }
      },
      {
        "screenId": 2,
        "screen": {
          "id": 2,
          "name": "Food Court Screen",
          "location": "3rd Floor"
        }
      }
    ],
    "campaignAds": [
      {
        "adId": 1,
        "playOrder": 1,
        "ad": {
          "id": 1,
          "title": "Summer Sale Promo",
          "durationSeconds": 30
        }
      },
      {
        "adId": 2,
        "playOrder": 2,
        "ad": {
          "id": 2,
          "title": "Product Launch",
          "durationSeconds": 15
        }
      }
    ]
  }
]
```

---

### Create Campaign

**POST** `/api/campaigns`

Create a new campaign schedule.

**Request Body:**
```json
{
  "name": "Summer Campaign 2026",
  "startTime": "2026-06-01T00:00:00Z",
  "endTime": "2026-08-31T23:59:59Z",
  "playOrder": 1,
  "screenIds": [1, 2],
  "adIds": [1, 2, 3]
}
```

**Field Descriptions:**
- `name` (required): Campaign name
- `startTime` (required): Campaign start time (ISO 8601 UTC)
- `endTime` (required): Campaign end time (ISO 8601 UTC)
- `playOrder` (optional): Priority order (lower plays first, default: 1)
- `screenIds` (required): Array of screen IDs to display on
- `adIds` (required): Array of ad IDs to include in campaign

**Response: 201 Created**
```json
{
  "id": 1,
  "name": "Summer Campaign 2026",
  "startTime": "2026-06-01T00:00:00Z",
  "endTime": "2026-08-31T23:59:59Z",
  "playOrder": 1,
  "campaignScreens": [...],
  "campaignAds": [...]
}
```

**Response: 400 Bad Request**
```json
{
  "errors": {
    "Name": ["The Name field is required."],
    "ScreenIds": ["At least one screen must be selected."]
  }
}
```

---

### Update Campaign

**PUT** `/api/campaigns/{id}`

Update an existing campaign.

**Parameters:**
- `id` (path, required): Campaign ID to update

**Request Body:**
```json
{
  "name": "Updated Campaign Name",
  "startTime": "2026-07-01T00:00:00Z",
  "endTime": "2026-09-30T23:59:59Z",
  "playOrder": 2,
  "screenIds": [1, 3],
  "adIds": [2, 4]
}
```

**Response: 200 OK**
```json
{
  "id": 1,
  "name": "Updated Campaign Name",
  "startTime": "2026-07-01T00:00:00Z",
  "endTime": "2026-09-30T23:59:59Z",
  "playOrder": 2,
  "campaignScreens": [...],
  "campaignAds": [...]
}
```

**Response: 404 Not Found**

---

### Delete Campaign

**DELETE** `/api/campaigns/{id}`

Delete a campaign and all its associations.

**Parameters:**
- `id` (path, required): Campaign ID to delete

**Request:**
```http
DELETE /api/campaigns/1 HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**

**Response: 404 Not Found**

---

## 🎵 Playlist API

### Get Screen Playlist

**GET** `/api/screens/{screenId}/playlist`

Get the ordered playlist of ads for a specific screen at a given time.

**Parameters:**
- `screenId` (path, required): Screen ID
- `at` (query, optional): ISO 8601 timestamp (defaults to current UTC time)

**Request Example 1: Current Time**
```http
GET /api/screens/1/playlist HTTP/1.1
Host: localhost:5085
```

**Request Example 2: Specific Time**
```http
GET /api/screens/1/playlist?at=2026-07-15T14:30:00Z HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**
```json
[
  {
    "adId": 1,
    "title": "Summer Sale Promo",
    "mediaUrl": "/uploads/ad_20260115_103045.mp4",
    "durationSeconds": 30,
    "mediaType": "video/mp4",
    "playOrder": 1
  },
  {
    "adId": 2,
    "title": "Product Launch",
    "mediaUrl": "/uploads/ad_20260115_110512.jpg",
    "durationSeconds": 15,
    "mediaType": "image/jpeg",
    "playOrder": 2
  },
  {
    "adId": 3,
    "title": "Brand Awareness",
    "mediaUrl": "/uploads/ad_20260116_083022.mp4",
    "durationSeconds": 45,
    "mediaType": "video/mp4",
    "playOrder": 3
  }
]
```

**Response: 200 OK (Empty Playlist)**
```json
[]
```

**Response: 404 Not Found**
```json
{
  "error": "Screen not found"
}
```

**Logic:**
1. Finds all campaigns that:
   - Include the specified screen
   - Are active at the given timestamp (startTime <= at <= endTime)
2. Collects all ads from matching campaigns
3. Orders by:
   - Campaign `playOrder` (ascending)
   - Ad `playOrder` within campaign (ascending)
4. Returns deduplicated list

**Use Case:**
Screen devices call this endpoint periodically to fetch their current playlist.

```javascript
// Example screen device code
setInterval(async () => {
  const playlist = await fetch(
    'http://localhost:5085/api/screens/1/playlist'
  ).then(r => r.json());
  
  playAds(playlist);
}, 300000); // Check every 5 minutes
```

---

## 📊 Proof of Play API

### Record Playback

**POST** `/api/proofofplay`

Record that an ad was played on a screen.

**Request Body:**
```json
{
  "screenId": 1,
  "adId": 1
}
```

**Field Descriptions:**
- `screenId` (required): ID of screen where ad played
- `adId` (required): ID of ad that was played
- `playedAt`: Automatically set to current UTC time

**Response: 200 OK**
```json
{
  "id": 1,
  "screenId": 1,
  "adId": 1,
  "playedAt": "2026-01-15T14:35:22Z"
}
```

**Response: 400 Bad Request**
```json
{
  "errors": {
    "ScreenId": ["The ScreenId field is required."],
    "AdId": ["The AdId field is required."]
  }
}
```

---

### Get Proof of Play Records

**GET** `/api/proofofplay`

Retrieve proof of play records with optional filters.

**Query Parameters:**
- `screenId` (optional): Filter by screen ID
- `startDate` (optional): Filter records after this date (ISO 8601)
- `endDate` (optional): Filter records before this date (ISO 8601)

**Request Example 1: All Records**
```http
GET /api/proofofplay HTTP/1.1
Host: localhost:5085
```

**Request Example 2: Filtered by Screen**
```http
GET /api/proofofplay?screenId=1 HTTP/1.1
Host: localhost:5085
```

**Request Example 3: Date Range**
```http
GET /api/proofofplay?startDate=2026-01-01&endDate=2026-01-31 HTTP/1.1
Host: localhost:5085
```

**Request Example 4: Combined Filters**
```http
GET /api/proofofplay?screenId=1&startDate=2026-01-15&endDate=2026-01-16 HTTP/1.1
Host: localhost:5085
```

**Response: 200 OK**
```json
[
  {
    "id": 1,
    "screenId": 1,
    "adId": 1,
    "playedAt": "2026-01-15T14:35:22Z",
    "screen": {
      "id": 1,
      "name": "Mall Entrance Display",
      "location": "Ground Floor"
    },
    "ad": {
      "id": 1,
      "title": "Summer Sale Promo",
      "durationSeconds": 30
    }
  },
  {
    "id": 2,
    "screenId": 1,
    "adId": 2,
    "playedAt": "2026-01-15T14:36:00Z",
    "screen": {
      "id": 1,
      "name": "Mall Entrance Display",
      "location": "Ground Floor"
    },
    "ad": {
      "id": 2,
      "title": "Product Launch",
      "durationSeconds": 15
    }
  }
]
```

---

## HTTP Status Codes

| Code | Meaning | Usage |
|------|---------|-------|
| 200 | OK | Successful GET, PUT, DELETE |
| 201 | Created | Successful POST (resource created) |
| 400 | Bad Request | Invalid input, validation errors |
| 404 | Not Found | Resource doesn't exist |
| 500 | Internal Server Error | Unexpected server error |

---

## Error Response Format

All error responses follow this format:

**Validation Errors (400):**
```json
{
  "errors": {
    "FieldName": ["Error message"],
    "AnotherField": ["Another error message"]
  }
}
```

**General Errors (404, 500):**
```json
{
  "error": "Error description"
}
```

---

## Rate Limiting

Currently not implemented. All endpoints are accessible without limits.

**Production Recommendation:** Implement rate limiting (e.g., 100 requests/minute per IP).

---

## CORS Configuration

Backend allows all origins for development:

```csharp
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
```

**Production Recommendation:** Restrict to specific frontend domain.

---

## Testing with cURL

### Create a Complete Workflow

**1. Create a Screen:**
```bash
curl -X POST "http://localhost:5085/api/screens" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Screen",
    "location": "Test Location",
    "status": "Active",
    "resolution": "1920x1080"
  }'
```

**2. Upload an Ad:**
```bash
curl -X POST "http://localhost:5085/api/ads/upload" \
  -F "file=@/path/to/video.mp4" \
  -F "title=Test Ad" \
  -F "durationSeconds=30"
```

**3. Create a Campaign:**
```bash
curl -X POST "http://localhost:5085/api/campaigns" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Campaign",
    "startTime": "2026-01-01T00:00:00Z",
    "endTime": "2026-12-31T23:59:59Z",
    "playOrder": 1,
    "screenIds": [1],
    "adIds": [1]
  }'
```

**4. Get Playlist:**
```bash
curl "http://localhost:5085/api/screens/1/playlist"
```

**5. Record Proof of Play:**
```bash
curl -X POST "http://localhost:5085/api/proofofplay" \
  -H "Content-Type: application/json" \
  -d '{
    "screenId": 1,
    "adId": 1
  }'
```

---

## Additional Resources

- **Swagger UI:** `http://localhost:5085/swagger` - Interactive API documentation
- **API Explorer:** Try all endpoints directly in browser
- **Source Code:** Check controller files for implementation details

---

**Last Updated:** January 16, 2026  
**API Version:** 1.0.0