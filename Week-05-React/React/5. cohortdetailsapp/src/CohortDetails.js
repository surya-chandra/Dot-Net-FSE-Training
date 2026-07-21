import React from 'react';
import styles from './CohortDetails.module.css';

export function CohortDetails({ cohort }) {
  const headerStyle = {
    color: cohort.status === 'ongoing' ? 'green' : 'blue'
  };

  return (
    <div className={styles.box}>
      <h3 style={headerStyle}>{cohort.name}</h3>
      <dl>
        <dt>Status:</dt>
        <dd>{cohort.status}</dd>
        <dt>Duration:</dt>
        <dd>{cohort.duration}</dd>
      </dl>
    </div>
  );
}
