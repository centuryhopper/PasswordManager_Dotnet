import { Col, Button, Row, Container, Card, Form } from "react-bootstrap";
import { useRouter } from "next/router";
import React from "react";
import Link from "next/link";

export const RegisterForm = () => {

  const router = useRouter()

  const handleClick = (e : React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault()
    router.push('/accounts')
  }

  const validate = () => {
    // check if username and password exists in user_accounts table.
    // if so, then success and grab the userId associated with the account. We will use this id to filter for the passwords that are stored by this user. Otherwise return failure
  }

  return (
    <div>
      <Container>
        <Row className="vh-100 d-flex justify-content-center align-items-center">
          <Col md={8} lg={6} xs={12}>
            <div className="border border-3 border-primary"></div>
            <Card className="shadow">
              <Card.Body>
                <div className="mb-3 mt-md-4">
                  <h2 className="fw-bold mb-2 text-uppercase ">Password Manager</h2>
                  <p className=" mb-5">Please create a memorable username and password.</p>
                  <div className="mb-3">
                    <Form>
                      <Form.Group className="mb-3" controlId="formBasicEmail">
                        <Form.Label className="text-center">
                          Username
                        </Form.Label>
                        <Form.Control type="email" placeholder="Enter username" />
                      </Form.Group>

                      <Form.Group
                        className="mb-3"
                        controlId="formBasicPassword"
                      >
                        <Form.Label>Password</Form.Label>
                        <Form.Control type="password" placeholder="Password" />
                      </Form.Group>

                      <Form.Group
                        className="mb-3"
                        controlId="formBasicCheckbox"
                      >
                      </Form.Group>
                      <div className="d-grid">
                        <Button variant="primary" type="submit" onClick={handleClick}>
                          Register
                        </Button>
                      </div>
                    </Form>
                    <div className="mt-3">
                      <p className="mb-0  text-center">
                        {"Already have an account? "}
                        <Link href="/accounts/login" className="text-primary fw-bold">
                          Login
                        </Link>
                      </p>
                    </div>
                  </div>
                </div>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </Container>
    </div>
  )
}