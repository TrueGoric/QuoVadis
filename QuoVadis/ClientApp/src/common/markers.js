import L from 'leaflet';
import drivingCar from '../assets/ev-front-fill.svg';
import drivingCarTransparent from '../assets/ev-front-fill-transparent.svg';

export const DrivingCar = new L.Icon({
    iconUrl: drivingCar,
    iconSize: [34, 71],
  });
  
export const DrivingCarTransparent = new L.Icon({
    iconUrl: drivingCarTransparent,
    iconSize: [34, 71],
  });