import React from 'react';
import '../Stylesheets/mystyle.css';

export function CalculateScore({ Name, School, Total, Goal }) {
  const average = Total / 4;
  return (
    <div className="formatstyle">
      <h1><font color="Brown">Student Details:</font></h1>
      <div className="Name"><b>Name:</b> <span>{Name}</span></div>
      <div className="School"><b>School:</b> <span>{School}</span></div>
      <div className="Total"><b>Total:</b> <span>{Total}</span></div>
      <div className="Score"><b>Score:</b> <span>{average}</span></div>
    </div>
  );
}
