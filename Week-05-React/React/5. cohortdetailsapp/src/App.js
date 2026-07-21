import React from 'react';
import { CohortDetails } from './CohortDetails';

function App() {
  const cohorts = [
    { name: 'Java FSE', status: 'ongoing', duration: '8 weeks' },
    { name: 'DotNet FSE', status: 'completed', duration: '12 weeks' }
  ];

  return (
    <div>
      {cohorts.map((c, i) => (
        <CohortDetails key={i} cohort={c} />
      ))}
    </div>
  );
}

export default App;
