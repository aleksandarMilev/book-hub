import React from 'react';
import { Container, Row, Col, Card, Form, Button } from 'react-bootstrap';

export default function BookForm() {
    return (
        <Container fluid className="mt-5">
            <Row className="justify-content-center">
                <Col md={8}>
                    <Card className="shadow-lg">
                        <Card.Body>
                            <h3 className="text-center mb-4">Add a New Book</h3>
                            <Form>
                                <Form.Group className="mb-3">
                                    <Form.Label>Title</Form.Label>
                                    <Form.Control 
                                        type="text" 
                                        placeholder="Enter book title" 
                                        required 
                                    />
                                </Form.Group>
                                <Form.Group className="mb-3">
                                    <Form.Label>Author</Form.Label>
                                    <Form.Control 
                                        type="text" 
                                        placeholder="Enter author's name" 
                                        required 
                                    />
                                </Form.Group>
                                <Form.Group className="mb-3">
                                    <Form.Label>Description</Form.Label>
                                    <Form.Control 
                                        as="textarea" 
                                        rows={3} 
                                        placeholder="Enter book description" 
                                        required 
                                    />
                                </Form.Group>
                                <div className="text-center">
                                    <Button variant="primary" type="submit">
                                        Add Book
                                    </Button>
                                </div>
                            </Form>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
}
