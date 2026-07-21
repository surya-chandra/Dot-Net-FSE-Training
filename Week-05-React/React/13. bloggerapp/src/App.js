import React, { useState } from 'react';

function BookDetails() {
  return <div><h3>Book Details:</h3><p>React Simplified by Dan Abramov</p></div>;
}

function BlogDetails() {
  return <div><h3>Blog Details:</h3><p>Understanding Conditional Rendering in React</p></div>;
}

function CourseDetails() {
  return <div><h3>Course Details:</h3><p>Full Stack React Development</p></div>;
}

function App() {
  const [view, setView] = useState('book');

  let content;
  if (view === 'book') {
    content = <BookDetails />;
  } else if (view === 'blog') {
    content = <BlogDetails />;
  } else {
    content = <CourseDetails />;
  }

  return (
    <div style={{ padding: '20px' }}>
      <h1>Blogger App</h1>
      <button onClick={() => setView('book')}>Book Details</button>
      <button onClick={() => setView('blog')}>Blog Details</button>
      <button onClick={() => setView('course')}>Course Details</button>
      <hr />
      {content}
    </div>
  );
}

export default App;
