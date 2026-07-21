import React from 'react';

function App() {
  const heading = <h1>Office Space Rental App</h1>;
  const imageUrl = "https://via.placeholder.com/150";

  const officeSpaces = [
    { Name: 'DBS Heritage', Rent: 50000, Address: 'MG Road, Bangalore' },
    { Name: 'Cyber Towers', Rent: 75000, Address: 'HITEC City, Hyderabad' },
    { Name: 'Tech Park', Rent: 55000, Address: 'OMR, Chennai' }
  ];

  return (
    <div>
      {heading}
      <img src={imageUrl} alt="Office Space" />
      <h2>Available Spaces</h2>
      {officeSpaces.map((item, index) => {
        const rentStyle = {
          color: item.Rent < 60000 ? 'red' : 'green',
          fontWeight: 'bold'
        };
        return (
          <div key={index} style={{ border: '1px solid #ccc', margin: '10px', padding: '10px' }}>
            <h3>{item.Name}</h3>
            <p style={rentStyle}>Rent: Rs. {item.Rent}</p>
            <p>Address: {item.Address}</p>
          </div>
        );
      })}
    </div>
  );
}

export default App;
