import React from 'react';
import { CalculateScore } from './Components/CalculateScore';

function App() {
  return (
    <div>
      <CalculateScore Name="John Doe" School="ABC High School" Total={360} Goal={90} />
    </div>
  );
}

export default App;
