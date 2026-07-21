import React from 'react';

export function ListofPlayers() {
  const players = [
    { name: 'Virat Kohli', score: 85 },
    { name: 'Rohit Sharma', score: 92 },
    { name: 'KL Rahul', score: 45 },
    { name: 'Shreyas Iyer', score: 68 },
    { name: 'Rishabh Pant', score: 78 },
    { name: 'Hardik Pandya', score: 55 },
    { name: 'Ravindra Jadeja', score: 62 },
    { name: 'Jasprit Bumrah', score: 15 },
    { name: 'Mohammed Shami', score: 20 },
    { name: 'Kuldeep Yadav', score: 10 },
    { name: 'Mohammed Siraj', score: 5 }
  ];

  const highScorers = players.filter(player => player.score >= 70);

  return (
    <div>
      <h2>All Players</h2>
      <ul>
        {players.map((p, index) => (
          <li key={index}>{p.name} - {p.score}</li>
        ))}
      </ul>
      <h2>High Scorers (Score &gt;= 70)</h2>
      <ul>
        {highScorers.map((p, index) => (
          <li key={index}>{p.name} - {p.score}</li>
        ))}
      </ul>
    </div>
  );
}

export function IndianPlayers() {
  const T20players = ['Surya', 'Ishan', 'Sanju'];
  const RanjiTrophy = ['Pujara', 'Rahane'];
  const mergedPlayers = [...T20players, ...RanjiTrophy];

  const [first, second, ...rest] = mergedPlayers;

  return (
    <div>
      <h2>Merged Squad</h2>
      <ul>
        {mergedPlayers.map((name, idx) => (
          <li key={idx}>{name}</li>
        ))}
      </ul>
      <p>Odd Team (First Player): {first}</p>
      <p>Even Team (Second Player): {second}</p>
    </div>
  );
}
