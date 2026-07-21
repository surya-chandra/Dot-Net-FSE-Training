import React, { useState } from 'react';

function GuestPage() {
  return (
    <div>
      <h2>Flight Details (Guest View)</h2>
      <p>Flight AI-101: Delhi to Mumbai - Rs. 5000</p>
      <p>Flight AI-202: Bangalore to Delhi - Rs. 6500</p>
      <p>Please log in to book tickets.</p>
    </div>
  );
}

function UserPage() {
  return (
    <div>
      <h2>Welcome Back, User! (Booking Portal)</h2>
      <p>Flight AI-101: <button onClick={() => alert("Ticket Booked!")}>Book Now</button></p>
      <p>Flight AI-202: <button onClick={() => alert("Ticket Booked!")}>Book Now</button></p>
    </div>
  );
}

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  return (
    <div style={{ padding: '20px' }}>
      <h1>Ticket Booking App</h1>
      {isLoggedIn ? (
        <div>
          <button onClick={() => setIsLoggedIn(false)}>Logout</button>
          <UserPage />
        </div>
      ) : (
        <div>
          <button onClick={() => setIsLoggedIn(true)}>Login</button>
          <GuestPage />
        </div>
      )}
    </div>
  );
}

export default App;
