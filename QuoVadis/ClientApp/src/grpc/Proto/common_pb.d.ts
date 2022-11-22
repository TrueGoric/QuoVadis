import * as jspb from 'google-protobuf'



export class VehicleInfo extends jspb.Message {
  getRegistration(): string;
  setRegistration(value: string): VehicleInfo;

  getModel(): string;
  setModel(value: string): VehicleInfo;

  getCostperkilometer(): string;
  setCostperkilometer(value: string): VehicleInfo;

  getLocation(): Location | undefined;
  setLocation(value?: Location): VehicleInfo;
  hasLocation(): boolean;
  clearLocation(): VehicleInfo;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): VehicleInfo.AsObject;
  static toObject(includeInstance: boolean, msg: VehicleInfo): VehicleInfo.AsObject;
  static serializeBinaryToWriter(message: VehicleInfo, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): VehicleInfo;
  static deserializeBinaryFromReader(message: VehicleInfo, reader: jspb.BinaryReader): VehicleInfo;
}

export namespace VehicleInfo {
  export type AsObject = {
    registration: string,
    model: string,
    costperkilometer: string,
    location?: Location.AsObject,
  }
}

export class Location extends jspb.Message {
  getLatitude(): number;
  setLatitude(value: number): Location;

  getLongitude(): number;
  setLongitude(value: number): Location;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Location.AsObject;
  static toObject(includeInstance: boolean, msg: Location): Location.AsObject;
  static serializeBinaryToWriter(message: Location, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Location;
  static deserializeBinaryFromReader(message: Location, reader: jspb.BinaryReader): Location;
}

export namespace Location {
  export type AsObject = {
    latitude: number,
    longitude: number,
  }
}

