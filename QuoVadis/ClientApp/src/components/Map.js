import { useEffect, useState } from "react";
import { Area } from "../common/const";

import { MonitorClient } from '../grpc/Proto/monitor_service_grpc_web_pb'
import { MonitorLocationRequest } from "../grpc/Proto/monitor_service_pb";

import { RealTimeView } from "./RealTimeView";

export const Map = () => {
    const [vehicles, setVehicles] = useState({});

    useEffect(() => {
        const monitorClient = new MonitorClient('');

        let monitorRequest = new MonitorLocationRequest();
        monitorRequest.setArea(Area);

        let stream = monitorClient.monitorLocation(monitorRequest);

        stream.on('data', (event) => {
            setVehicles(vehicles => {
                let newVehicles = { ...vehicles };
                let registrationNumber = event.getRegistrationnumber();

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
    }, []);

    return <RealTimeView vehicles={vehicles} />;
};