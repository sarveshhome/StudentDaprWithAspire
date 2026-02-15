# Quick Start Guide - Blazor Web Application

## 🚀 Getting Started

### Step 1: Start the Student API

```bash
# Terminal 1 - Start the API
cd StudentDaprWithAspire
dotnet run --project StudentDaprWithAspire.API
```

API will be available at: http://localhost:5000

### Step 2: Start the Blazor App

```bash
# Terminal 2 - Start the Blazor app
cd StudentDaprWithAspire
dotnet run --project StudentDaprWithAspire.WebBlazor
```

Blazor app will be available at:
- HTTP: http://localhost:5001
- HTTPS: https://localhost:7001

### Step 3: Open in Browser

Navigate to: https://localhost:7001

---

## 📱 Using the Application

### View Students
1. Click **"Students"** in the navigation menu
2. View list of all students

### Create Student
1. Go to Students page
2. Click **"Add New Student"** button
3. Fill in the form:
   - Name: Student name
   - Email: Student email
   - Age: Student age
4. Click **"Create Student"**

### Edit Student
1. Go to Students page
2. Click **"Edit"** button next to a student
3. Modify the fields
4. Click **"Update Student"**

### Delete Student
1. Go to Students page
2. Click **"Delete"** button next to a student
3. Student will be removed

---

## 🎨 UI Features

### Navigation
- **Home** - Welcome page with system overview
- **Students** - Student management page
- **Counter** - Demo counter page
- **Weather** - Demo weather page

### Student List Page
- Table view of all students
- **Add New Student** button
- **Refresh** button to reload data
- **Edit** button for each student
- **Delete** button for each student

### Forms
- Real-time validation
- Loading indicators
- Success/error messages
- Cancel button to go back

---

## ⚙️ Configuration

### Change API URL

Edit `appsettings.json`:

```json
{
  "StudentApiUrl": "http://localhost:5000"
}
```

Or set environment variable:

```bash
export StudentApiUrl="http://your-api-url"
```

---

## 🔧 Troubleshooting

### Cannot Connect to API

**Error**: "Failed to load students"

**Solution**:
1. Verify API is running: http://localhost:5000/api/students
2. Check `StudentApiUrl` in appsettings.json
3. Ensure no firewall blocking

### Page Not Loading

**Error**: Blank page or loading forever

**Solution**:
1. Check browser console (F12) for errors
2. Verify .NET 10.0 SDK is installed
3. Clear browser cache and reload

### SignalR Connection Failed

**Error**: "Reconnecting..." message

**Solution**:
1. Refresh the page
2. Check network connectivity
3. Restart the Blazor app

---

## 📊 Sample Data

### Create Sample Students

Use the API or Blazor UI to create:

```
Name: John Doe
Email: john.doe@example.com
Age: 20

Name: Jane Smith
Email: jane.smith@example.com
Age: 22

Name: Bob Johnson
Email: bob.johnson@example.com
Age: 21
```

---

## 🎯 Key Features Demonstrated

### Blazor Server
- ✅ Interactive components
- ✅ Real-time updates
- ✅ Server-side rendering
- ✅ SignalR connection

### HTTP Client
- ✅ GET requests
- ✅ POST requests
- ✅ PUT requests
- ✅ DELETE requests
- ✅ Error handling

### UI/UX
- ✅ Bootstrap 5 styling
- ✅ Responsive design
- ✅ Loading states
- ✅ Form validation
- ✅ Success/error messages

---

## 🚀 Next Steps

1. **Add Authentication**
   - Implement user login
   - Role-based access control

2. **Enhance UI**
   - Add search functionality
   - Implement pagination
   - Add sorting

3. **Real-time Updates**
   - Use SignalR for live updates
   - Subscribe to Dapr events

4. **Testing**
   - Add bUnit tests
   - Integration tests
   - E2E tests

---

## 📚 Learn More

- [Blazor Tutorial](https://docs.microsoft.com/aspnet/core/blazor/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/)
- [HttpClient Best Practices](https://docs.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines)

---

## 💡 Tips

1. **Use Browser DevTools** (F12) to debug
2. **Check Network Tab** for API calls
3. **Monitor Console** for errors
4. **Use Hot Reload** for faster development
5. **Test with Different Browsers**

---

## 🎉 Success!

You now have a fully functional Blazor web application that:
- Displays students in a table
- Creates new students
- Updates existing students
- Deletes students
- Handles errors gracefully
- Provides a great user experience

Enjoy building with Blazor! 🚀
