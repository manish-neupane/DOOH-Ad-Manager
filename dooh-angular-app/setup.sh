#!/bin/bash

echo "🚀 Setting up DOOH Angular Application..."
echo ""

# Check if Node.js is installed
if ! command -v node &> /dev/null
then
    echo "❌ Node.js is not installed. Please install Node.js v18 or higher."
    exit 1
fi

echo "✅ Node.js version: $(node -v)"

# Check if npm is installed
if ! command -v npm &> /dev/null
then
    echo "❌ npm is not installed. Please install npm."
    exit 1
fi

echo "✅ npm version: $(npm -v)"

# Check if Angular CLI is installed
if ! command -v ng &> /dev/null
then
    echo "⚠️  Angular CLI not found globally. Installing..."
    npm install -g @angular/cli@19
else
    echo "✅ Angular CLI version: $(ng version --minimal)"
fi

# Install dependencies
echo ""
echo "📦 Installing dependencies..."
npm install

# Check if installation was successful
if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Setup completed successfully!"
    echo ""
    echo "📝 Next steps:"
    echo "1. Make sure your .NET backend is running on http://localhost:5085"
    echo "2. Start the Angular app with: npm start"
    echo "3. Open your browser at: http://localhost:4200"
    echo ""
else
    echo ""
    echo "❌ Installation failed. Please check the errors above."
    exit 1
fi
