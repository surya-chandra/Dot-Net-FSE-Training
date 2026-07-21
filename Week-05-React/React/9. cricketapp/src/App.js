import React from 'react';
import { ListofPlayers, IndianPlayers } from './CricketComponents';

function App() {
  const flag = true;

  return (
    <div>
      {flag ? <ListofPlayers /> : <IndianPlayers />}
    </div>
  );
}

export default App;
