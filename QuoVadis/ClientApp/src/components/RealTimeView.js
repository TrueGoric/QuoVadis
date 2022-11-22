import { MapContainer, TileLayer } from "react-leaflet";
import { Col, Container, Row } from "reactstrap";
import { Center, ZoomLevel } from "../common/const";

import './RealTimeView.css';
import { VehicleMarker } from "./VehicleMarker";

export const RealTimeView = ({ vehicles }) => {
  return (
    <Container>
      <Row>
        <Col>
          <div className="map" id="map">
            <MapContainer center={Center} zoom={ZoomLevel} scrollWheelZoom={true}>
              <TileLayer
                attribution='&copy; <a href="https://stadiamaps.com/">Stadia Maps</a>, &copy; <a href="https://openmaptiles.org/">OpenMapTiles</a> &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors'
                url='https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/{z}/{x}/{y}{r}.png'
              />
              {Object.keys(vehicles).map(registration => {
                let vehicle = vehicles[registration];
                return (<VehicleMarker key={registration} registration={registration} {...vehicle}/>);
              })}
            </MapContainer>
          </div>
        </Col>
      </Row>
    </Container>
  );
};