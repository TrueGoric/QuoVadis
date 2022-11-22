import { useState } from "react";
import { useCookies } from 'react-cookie';
import { Button, Col, Container, Form, FormGroup, Input, Label, Row, Spinner } from "reactstrap";
import { UserClient } from '../grpc/Proto/user_service_grpc_web_pb';
import { LoginRequest, RegisterRequest, RegistrationResult } from "../grpc/Proto/user_service_pb";

export const Login = () => {
    const [login, setLogin] = useState({ username: '', password: '', loading: false, error: false });
    const [register, setRegister] = useState({ username: '', password: '', passwordConfirm: '', loading: false, errUser: false, errPass: false, success: false });
    const [cookies, setCookie] = useCookies(['username']);

    function handleLogin(event) {
        setLogin({ ...login, loading: true });

        var userClient = new UserClient('');
        var loginRequest = new LoginRequest();
        loginRequest.setUsername(login.username);
        loginRequest.setPassword(login.password);

        userClient.login(loginRequest, undefined, (err, response) => {
            if (response.getSuccess()) {
                setCookie('username', login.username);
            }
            else {
                setLogin({ ...login, loading: false, error: true });
            }
        });
    }

    function handleRegister(event) {
        setRegister({ ...register, loading: true, errUser: false, errPass: false });

        if (register.password !== register.passwordConfirm || register === '') {
            setRegister({ ...register, errPass: true });
            return;
        }

        if (register.username === '') {
            setRegister({ ...register, errUser: true });
            return;
        }

        var userClient = new UserClient('');
        var registerRequest = new RegisterRequest();
        registerRequest.setUsername(register.username);
        registerRequest.setPassword(register.password);

        userClient.register(registerRequest, undefined, (err, response) => {
            if (response.getResult() === RegistrationResult.SUCCESSFULLY_REGISTERED) {
                setRegister({ ...register, loading: false, success: true });
            }
            else if (response.getResult() === RegistrationResult.USERNAME_IN_USE) {
                setRegister({ ...register, loading: false, errUser: true });
            }
        });
    }

    return (
        <Container>
            <Row className='justify-content-center'>
                <Col md={{ size: 5 }} sm={{ size: 7 }}>
                    <Form>
                        <FormGroup>
                            <Label for="username">
                                Username
                            </Label>
                            <Input
                                id="username"
                                name="username"
                                type="text"
                                value={login.username}
                                disabled={login.loading}
                                invalid={login.error}
                                onChange={(e) => setLogin({ ...login, username: e.target.value })}
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label for="password">
                                Password
                            </Label>
                            <Input
                                id="password"
                                name="password"
                                type="password"
                                value={login.password}
                                disabled={login.loading}
                                invalid={login.error}
                                onChange={(e) => setLogin({ ...login, password: e.target.value })}
                            />
                        </FormGroup>
                        <Button className="btn-primary" onClick={handleLogin} disabled={login.loading}>
                            {login.loading && <Spinner className="spinner-border spinner-border-sm" />}
                            Login
                        </Button>
                    </Form>
                    <p style={{ textAlign: 'center', padding: '10pt' }}>
                        OR
                    </p>
                    <Form>
                        <FormGroup>
                            <Label for="usernameRegister">
                                Username
                            </Label>
                            <Input
                                id="usernameRegister"
                                name="usernameRegister"
                                placeholder="i.e. AwesomeDriver332"
                                type="text"
                                value={register.username}
                                disabled={register.loading}
                                invalid={register.errUser}
                                onChange={(e) => setRegister({ ...register, username: e.target.value })}
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label for="passwordRegister">
                                Password
                            </Label>
                            <Input
                                id="passwordRegister"
                                name="passwordRegister"
                                type="password"
                                value={register.password}
                                disabled={register.loading}
                                invalid={register.errPass}
                                onChange={(e) => setRegister({ ...register, password: e.target.value })}
                            />
                        </FormGroup>
                        <FormGroup>
                            <Label for="passwordConfirmRegister">
                                Repeat password
                            </Label>
                            <Input
                                id="passwordConfirmRegister"
                                name="passwordConfirmRegister"
                                type="password"
                                value={register.passwordConfirm}
                                disabled={register.loading}
                                invalid={register.errPass}
                                onChange={(e) => setRegister({ ...register, passwordConfirm: e.target.value })}
                            />
                        </FormGroup>
                        <Button onClick={handleRegister} disabled={register.loading || register.success}>
                            {register.loading && <Spinner className="spinner-border spinner-border-sm" />}
                            {!register.success ? 'Register' : 'Registration successful'}
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Container>
    );
};