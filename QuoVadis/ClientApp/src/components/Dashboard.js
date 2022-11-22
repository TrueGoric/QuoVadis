import { useEffect, useState } from "react";
import { Button, Col, Container, Form, Input, InputGroup, InputGroupText, Row } from "reactstrap";

import { Area } from "../common/const";

import { MonitorClient } from '../grpc/Proto/monitor_service_grpc_web_pb'
import { MonitorLocationRequest } from "../grpc/Proto/monitor_service_pb";
import { RentClient } from '../grpc/Proto/rent_service_grpc_web_pb';
import { BeginRentRequest, GetVehiclesRequest, GetCurrentlyRentedVehicleRequest, BeginRentStatus, EndRentRequest, EndRentStatus } from '../grpc/Proto/rent_service_pb';
import { UserClient } from '../grpc/Proto/user_service_grpc_web_pb';
import { AddFundsRequest, CheckBalanceRequest } from "../grpc/Proto/user_service_pb";
import { VehicleClient } from '../grpc/Proto/vehicle_service_grpc_web_pb';
import { UpdatePositionRequest } from "../grpc/Proto/vehicle_service_pb";
import { Location } from "../grpc/Proto/common_pb";

import { RealTimeView } from "./RealTimeView";

import { computeDestinationPoint } from 'geolib';
import useInterval from "../common/useInterval";


export const Dashboard = () => {
    const [vehicles, setVehicles] = useState({});
    const [trackedVehicles, setTrackedVehicles] = useState({});

    const [balance, setBalance] = useState('0.0');
    const [upBalance, setUpBalance] = useState('0.0');
    const [currentVehicle, setCurrentVehicle] = useState({ registrationNumber: '' });

    function checkBalance() {
        let userClient = new UserClient('');
        let checkRequest = new CheckBalanceRequest();

        userClient.checkBalance(checkRequest, undefined, (err, response) => {
            setBalance(response.getTotal());
        });
    }

    function addBalance() {
        let userClient = new UserClient('');
        let addRequest = new AddFundsRequest();
        addRequest.setAmount(upBalance);

        userClient.addFunds(addRequest, undefined, (err, response) => {
            setUpBalance('');
            checkBalance();
        });
    }

    function checkRent() {
        const rentClient = new RentClient('');

        let checkRequest = new GetCurrentlyRentedVehicleRequest();

        rentClient.getCurrentlyRentedVehicle(checkRequest, undefined, (err, response) => {
            if (response.hasVehicle()) {
                var vehicle = response.getVehicle();
                setCurrentVehicle({
                    registrationNumber: vehicle.getRegistration(),
                    model: vehicle.getModel(),
                    costPerKilometer: vehicle.getCostperkilometer(),
                    latitude: vehicle.getLocation().getLatitude(),
                    longitude: vehicle.getLocation().getLongitude(),
                });
            }
            else {
                setCurrentVehicle({ registrationNumber: '' });
            }
        })
    }

    function checkVehiclesInTheArea() {
        const rentClient = new RentClient('');

        let vehicleListRequest = new GetVehiclesRequest();
        vehicleListRequest.setArea(Area);

        rentClient.getVehicles(vehicleListRequest, undefined, (err, response) => {
            if (!err) {
                let vehicles = {};

                response.getVehiclesList().forEach(vehicle => {
                    vehicles[vehicle.getRegistration()] = {
                        model: vehicle.getModel(),
                        costPerKilometer: vehicle.getCostperkilometer(),
                        latitude: vehicle.getLocation().getLatitude(),
                        longitude: vehicle.getLocation().getLongitude(),
                        rentable: true,
                        rentAction: (r) => rentVehicle(r),
                        transparent: false
                    };
                });

                setVehicles(vehicles);
            }
        });

    }

    function rentVehicle(registrationNumber) {
        const rentClient = new RentClient('');

        let rentRequest = new BeginRentRequest();
        rentRequest.setRegistrationnumber(registrationNumber);

        rentClient.beginRent(rentRequest, undefined, (err, response) => {
            var status = response.getStatus();

            if (status === BeginRentStatus.RENT_IN_PROGRESS) {
                alert('You are already renting a vehicle!');
            }
            else if (status === BeginRentStatus.INSUFFICIENT_FUNDS) {
                alert('There are insufficient funds to rent!');
            }
            else if (status === BeginRentStatus.VEHICLE_DOESNT_EXIST) {
                alert('There\'s no such vehicle!');
            }
            else if (status === BeginRentStatus.VEHICLE_IN_USE) {
                alert('This vehicle has been already rented!');
                checkVehiclesInTheArea();
            }

            checkRent();
            checkBalance();
        });
    }

    function returnVehicle() {
        const rentClient = new RentClient('');

        let rentRequest = new EndRentRequest();

        rentClient.endRent(rentRequest, undefined, (err, response) => {
            var status = response.getStatus();

            if (status === EndRentStatus.NO_VEHICLE_RENTED) {
                alert('There\'s no rent in progress!');
            }
            else if (status === EndRentStatus.OUTSIDE_AREA) {
                alert('The vehicle is parked outside municipality! Return and try again!');
            }

            checkRent();
            checkBalance();
        });
    }

    function steerVehicle(e) {
        if (currentVehicle.registrationNumber === '')
            return;

        if (e.key === 'w') {
            var newPosition = computeDestinationPoint({ latitude: currentVehicle.latitude, longitude: currentVehicle.longitude }, 100, 0);
            e.preventDefault();
        }
        else if (e.key === 'd') {
            var newPosition = computeDestinationPoint({ latitude: currentVehicle.latitude, longitude: currentVehicle.longitude }, 100, 90);
            e.preventDefault();
        }
        else if (e.key === 's') {
            var newPosition = computeDestinationPoint({ latitude: currentVehicle.latitude, longitude: currentVehicle.longitude }, 100, 180);
            e.preventDefault();
        }
        else if (e.key === 'a') {
            var newPosition = computeDestinationPoint({ latitude: currentVehicle.latitude, longitude: currentVehicle.longitude }, 100, 270);
            e.preventDefault();
        }
        else {
            return;
        }

        setCurrentVehicle(currentVehicle => {
            let newCurrent = { ...currentVehicle };

            newCurrent.latitude = newPosition.latitude;
            newCurrent.longitude = newPosition.longitude;

            return newCurrent;
        })

        setTrackedVehicles(trackedVehicles => {
            let newVehicles = { ...trackedVehicles };

            newVehicles[currentVehicle.registrationNumber] = {
                latitude: newPosition.latitude,
                longitude: newPosition.longitude,
                transparent: false
            };

            return newVehicles;
        });
    }

    useInterval(() => {
        if (currentVehicle.registrationNumber !== '') {
            let vehicleClient = new VehicleClient('');
            let updateRequest = new UpdatePositionRequest();
            let location = new Location();

            location.setLatitude(currentVehicle.latitude);
            location.setLongitude(currentVehicle.longitude);
            updateRequest.setVehicleRegistration(currentVehicle.registrationNumber);
            updateRequest.setLocation(location);

            vehicleClient.updatePosition(updateRequest, undefined, (e, r) => { });
        }
    }, 250);

    useEffect(() => {
        checkBalance();
        checkRent();
    }, []);

    useEffect(() => {
        if (currentVehicle.registrationNumber === '') {
            checkVehiclesInTheArea();
        }
        else {
            const monitorClient = new MonitorClient('');

            let monitorRequest = new MonitorLocationRequest();
            monitorRequest.setArea(Area);

            let stream = monitorClient.monitorLocation(monitorRequest);

            setTrackedVehicles({
                [currentVehicle.registrationNumber]: {
                    latitude: currentVehicle.latitude,
                    longitude: currentVehicle.longitude,
                    transparent: false
                }
            });

            stream.on('data', (event) => {
                setTrackedVehicles(trackedVehicles => {
                    let registrationNumber = event.getRegistrationnumber();

                    if (registrationNumber === currentVehicle.registrationNumber)
                        return trackedVehicles;

                    let newVehicles = { ...trackedVehicles };

                    if (event.hasLocation()) {
                        let location = event.getLocation();
                        newVehicles[registrationNumber] = {
                            latitude: location.getLatitude(),
                            longitude: location.getLongitude(),
                            transparent: true
                        };
                    }
                    else {
                        newVehicles.remove(registrationNumber);
                    }

                    return newVehicles;
                })
            });

            return () => {
                stream.cancel();
            };
        }
    }, [currentVehicle.registrationNumber]);

    return (
        <div>
            <Container onKeyDown={steerVehicle}>
                <Row>
                    <Col className="text-center align-middle"><h3>Balance: ${balance}</h3></Col>
                    <Col className="text-center align-middle">
                        <Form>
                            <InputGroup>
                                <InputGroupText>
                                    $
                                </InputGroupText>
                                <Input placeholder="amount of $ to add to the account" value={upBalance} onChange={e => setUpBalance(e.target.value)} onKeyPress={(e) => { if (e.key === 'Enter') { addBalance(); e.preventDefault(); } }} />
                                <Button onClick={addBalance}>Add funds</Button>
                            </InputGroup>
                        </Form>
                    </Col>
                    {currentVehicle.registrationNumber !== '' && <Col className="text-center align-middle">
                        <Button className="btn-primary" onClick={returnVehicle}>End rent</Button>
                    </Col>}
                    {currentVehicle.registrationNumber === '' && <Col className="text-center align-middle">
                        <Button onClick={checkVehiclesInTheArea}>Refresh map</Button>
                    </Col>}
                </Row>
                <Row>
                    <Col>
                        {currentVehicle.registrationNumber !== '' ? <RealTimeView vehicles={trackedVehicles} /> : <RealTimeView vehicles={vehicles} />}
                    </Col>
                </Row>
                {currentVehicle.registrationNumber !== '' &&
                    <Row>
                        <Col className="text-center align-middle">
                            <h4>Steer your vehicle with W, A, S, D keys.</h4>
                        </Col>
                    </Row>
                }
            </Container>
        </div>
    );
};