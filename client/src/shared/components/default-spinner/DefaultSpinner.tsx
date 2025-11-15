import type { FC } from 'react';
import { Button, Spinner } from 'react-bootstrap';

const DefaultSpinner: FC = () => (
  <>
    <Button variant="primary" disabled>
      <Spinner as="span" animation="border" size="sm" role="status" aria-hidden="true" />
      <span className="visually-hidden">Loading...</span>
    </Button>{' '}
    <Button variant="primary" disabled>
      <Spinner as="span" animation="grow" size="sm" role="status" aria-hidden="true" />
      Loading...
    </Button>
  </>
);

export default DefaultSpinner;
