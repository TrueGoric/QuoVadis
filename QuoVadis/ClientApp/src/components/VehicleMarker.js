import { useCallback } from "react";
import { Marker, Popup } from "react-leaflet";
import { Button } from "reactstrap";
import { DebitLock } from "../common/const";
import { DrivingCar, DrivingCarTransparent } from "../common/markers";

export const VehicleMarker = ({ registration, model, latitude, longitude, rentable, rentAction, costPerKilometer, transparent }) => {
    const onClick = useCallback(
        () => {

            if (rentAction) {
                rentAction(registration);
            }
        },
        [rentAction, registration]
    );

    return (
        <Marker position={[latitude, longitude]} icon={transparent ? DrivingCarTransparent : DrivingCar} >
            {rentable && <Popup>
                Registration: {registration} <br />
                Model: <b>{model}</b>
                <br /><br />
                Rate per km: ${costPerKilometer} (min. balance to rent: ${costPerKilometer * DebitLock})
                <br /><br />
                <Button onClick={onClick}>Rent</Button>
            </Popup>}
        </Marker>
    );
}