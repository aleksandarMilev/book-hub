import React from 'react';
import { Container, Card, Row, Col, Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const CarryIcon = () => (
  <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
    <path d="M4 9H20V11H4V9Z" fill="#FFBF00" />
    <path d="M4 15H20V17H4V15Z" fill="#FFBF00" />
    <path d="M12 1L9 5H15L12 1Z" fill="#FFBF00" />
  </svg>
);

const DumbbellIcon = () => (
  <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
    <path d="M7 10V14H2V10H7ZM9 10V14H15V10H9ZM17 10V14H22V10H17ZM17 4H20V9H17V4ZM17 15H20V20H17V15ZM4 20H7V15H4V20ZM4 9H7V4H4V9Z" fill="#FFBF00" />
  </svg>
);

const ClockIcon = () => (
  <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
    <path d="M12 2C17.53 2 22 6.48 22 12C22 17.52 17.53 22 12 22C6.47 22 2 17.52 2 12C2 6.48 6.47 2 12 2ZM12 15H10V7H12V15ZM12 17H10V19H12V17Z" fill="#FFBF00" />
  </svg>
);

export default function SexyDevilsCarry() {
  return (
    <Container>
      <h3 className="text-center my-3" style={{ fontSize: '2rem', color: '#FFBF00' }}>
        <CarryIcon /> 🎃 Sexy Devils Carry
      </h3>

      <Row className="g-4">
        <Col>
          <Card className="text-center" style={{ backgroundColor: '#222', color: '#fff' }}>
            <Card.Body>
              <Card.Title style={{ fontSize: '1.5rem', color: '#FFBF00' }}>
                From 0:00 - 08:00
              </Card.Title>
              <Card.Text>
                <strong>6 Rounds:</strong><br />
                10m Farmer Carry / Back Carry<br />
                10 Devil Press (Synchro) (15/12.5 kg)<br />
                <strong>Max DB Thrusters (Synchro) until 08:00</strong>
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <Row className="g-4">
        <Col>
          <Card className="text-center" style={{ backgroundColor: '#222', color: '#fff' }}>
            <Card.Body>
              <Card.Title style={{ fontSize: '1.5rem', color: '#FFBF00' }}>
                From 08:00 - 12:00
              </Card.Title>
              <Card.Text>
                <strong>Find 1RM Clean and Jerk</strong>
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>
      </Row>
      <Row className="g-4">
        <Col>
          <Card className="text-center" style={{ backgroundColor: '#222', color: '#fff' }}>
            <Card.Body>
            <Card.Text style={{ fontSize: '1.2rem', marginTop: '1rem' }}>
                <strong>Score:</strong> 1RM kgs + Thrusters Count
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
}
