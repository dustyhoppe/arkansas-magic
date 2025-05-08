# Arkansas Magic - Local Development Setup

This project uses Docker and Docker Compose to spin up all required services for local development.

---

## 📦 Prerequisites

Before running the project, ensure you have the following tools installed:

### ✅ 1. Docker Desktop

- **Download**: [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)
- Follow the installation instructions for your operating system.
- After installation, **start Docker Desktop** and ensure it’s running.

### ✅ 2. Docker Compose

Docker Compose comes bundled with Docker Desktop (v2+), so no extra installation is needed. You can verify it’s installed by running:

```powershell
docker-compose --version
```

---

## ▶️ Running the Project

After installing Docker and ensuring it’s running:

```powershell
.\setup.ps1
```

This script starts up all required containers and builds any necessary images using `docker-compose`. It typically runs a command like:

```powershell
docker-compose -f docker-compose.yaml -f docker-compose.local-services.yaml up --build --detach
```

You can modify `setup.ps1` to include additional logic if needed (e.g., database seeding, file setup, etc.).

---

## ⚙️ Configuring Event Filtering

The application supports event filtering based on geographic proximity. You can configure this behavior in your `appsettings.json` file using the `EventFilters` section:

```json
"EventFilters": {
  "MaximumMiles": 180,
  "Coordinates": {
    "Latitude": 35.20105,
    "Longitude": -91.8318334
  }
}
```

### 🔧 Configuration Options:

- **MaximumMiles**  
  Sets the maximum distance (in miles) from the coordinates specified below. Events beyond this range will be filtered out.

- **Coordinates**
  - `Latitude`: The center latitude for the event filter.
  - `Longitude`: The center longitude for the event filter.

### 📁 Location

Be sure to place this configuration in the appropriate environment-specific settings file, such as:

```plaintext
appsettings.json
```
