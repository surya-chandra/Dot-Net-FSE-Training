import React, { useState } from 'react';

export function CurrencyConvertor() {
  const [rupees, setRupees] = useState(0);
  const [euro, setEuro] = useState(0);

  const handleSubmit = (e) => {
    e.preventDefault();
    setEuro(rupees / 90);
  };

  return (
    <form onSubmit={handleSubmit}>
      <h3>Currency Convertor</h3>
      <label>Rupees: </label>
      <input type="number" value={rupees} onChange={(e) => setRupees(Number(e.target.value))} />
      <button type="submit">Convert</button>
      <p>Euro: {euro.toFixed(2)}</p>
    </form>
  );
}

export function EventComponent() {
  const [counter, setCounter] = useState(0);

  const handleIncrement = () => {
    setCounter(prev => prev + 1);
    alert("Hello from Increment!");
  };

  const handleDecrement = () => {
    setCounter(prev => prev - 1);
  };

  const sayWelcome = (msg) => {
    alert(msg);
  };

  const handlePress = (e) => {
    alert("I was clicked");
  };

  return (
    <div>
      <h2>Counter: {counter}</h2>
      <button onClick={handleIncrement}>Increment</button>
      <button onClick={handleDecrement}>Decrement</button>
      <button onClick={() => sayWelcome("welcome")}>Say Welcome</button>
      <button onClick={handlePress}>OnPress Synthetic Event</button>
      <hr />
      <CurrencyConvertor />
    </div>
  );
}
