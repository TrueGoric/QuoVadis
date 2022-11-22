import * as jspb from 'google-protobuf'

import * as Proto_common_pb from '../Proto/common_pb';


export class GetAreasRequest extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): GetAreasRequest.AsObject;
  static toObject(includeInstance: boolean, msg: GetAreasRequest): GetAreasRequest.AsObject;
  static serializeBinaryToWriter(message: GetAreasRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): GetAreasRequest;
  static deserializeBinaryFromReader(message: GetAreasRequest, reader: jspb.BinaryReader): GetAreasRequest;
}

export namespace GetAreasRequest {
  export type AsObject = {
  }
}

export class GetAreasResponse extends jspb.Message {
  getAreasList(): Array<string>;
  setAreasList(value: Array<string>): GetAreasResponse;
  clearAreasList(): GetAreasResponse;
  addAreas(value: string, index?: number): GetAreasResponse;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): GetAreasResponse.AsObject;
  static toObject(includeInstance: boolean, msg: GetAreasResponse): GetAreasResponse.AsObject;
  static serializeBinaryToWriter(message: GetAreasResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): GetAreasResponse;
  static deserializeBinaryFromReader(message: GetAreasResponse, reader: jspb.BinaryReader): GetAreasResponse;
}

export namespace GetAreasResponse {
  export type AsObject = {
    areasList: Array<string>,
  }
}

export class GetVehiclesRequest extends jspb.Message {
  getArea(): string;
  setArea(value: string): GetVehiclesRequest;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): GetVehiclesRequest.AsObject;
  static toObject(includeInstance: boolean, msg: GetVehiclesRequest): GetVehiclesRequest.AsObject;
  static serializeBinaryToWriter(message: GetVehiclesRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): GetVehiclesRequest;
  static deserializeBinaryFromReader(message: GetVehiclesRequest, reader: jspb.BinaryReader): GetVehiclesRequest;
}

export namespace GetVehiclesRequest {
  export type AsObject = {
    area: string,
  }
}

export class GetVehiclesResponse extends jspb.Message {
  getVehiclesList(): Array<Proto_common_pb.VehicleInfo>;
  setVehiclesList(value: Array<Proto_common_pb.VehicleInfo>): GetVehiclesResponse;
  clearVehiclesList(): GetVehiclesResponse;
  addVehicles(value?: Proto_common_pb.VehicleInfo, index?: number): Proto_common_pb.VehicleInfo;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): GetVehiclesResponse.AsObject;
  static toObject(includeInstance: boolean, msg: GetVehiclesResponse): GetVehiclesResponse.AsObject;
  static serializeBinaryToWriter(message: GetVehiclesResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): GetVehiclesResponse;
  static deserializeBinaryFromReader(message: GetVehiclesResponse, reader: jspb.BinaryReader): GetVehiclesResponse;
}

export namespace GetVehiclesResponse {
  export type AsObject = {
    vehiclesList: Array<Proto_common_pb.VehicleInfo.AsObject>,
  }
}

export class GetCurrentlyRentedVehicleRequest extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): GetCurrentlyRentedVehicleRequest.AsObject;
  static toObject(includeInstance: boolean, msg: GetCurrentlyRentedVehicleRequest): GetCurrentlyRentedVehicleRequest.AsObject;
  static serializeBinaryToWriter(message: GetCurrentlyRentedVehicleRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): GetCurrentlyRentedVehicleRequest;
  static deserializeBinaryFromReader(message: GetCurrentlyRentedVehicleRequest, reader: jspb.BinaryReader): GetCurrentlyRentedVehicleRequest;
}

export namespace GetCurrentlyRentedVehicleRequest {
  export type AsObject = {
  }
}

export class GetCurrentlyRentedVehicleResponse extends jspb.Message {
  getVehicle(): Proto_common_pb.VehicleInfo | undefined;
  setVehicle(value?: Proto_common_pb.VehicleInfo): GetCurrentlyRentedVehicleResponse;
  hasVehicle(): boolean;
  clearVehicle(): GetCurrentlyRentedVehicleResponse;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): GetCurrentlyRentedVehicleResponse.AsObject;
  static toObject(includeInstance: boolean, msg: GetCurrentlyRentedVehicleResponse): GetCurrentlyRentedVehicleResponse.AsObject;
  static serializeBinaryToWriter(message: GetCurrentlyRentedVehicleResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): GetCurrentlyRentedVehicleResponse;
  static deserializeBinaryFromReader(message: GetCurrentlyRentedVehicleResponse, reader: jspb.BinaryReader): GetCurrentlyRentedVehicleResponse;
}

export namespace GetCurrentlyRentedVehicleResponse {
  export type AsObject = {
    vehicle?: Proto_common_pb.VehicleInfo.AsObject,
  }
}

export class BeginRentRequest extends jspb.Message {
  getRegistrationnumber(): string;
  setRegistrationnumber(value: string): BeginRentRequest;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): BeginRentRequest.AsObject;
  static toObject(includeInstance: boolean, msg: BeginRentRequest): BeginRentRequest.AsObject;
  static serializeBinaryToWriter(message: BeginRentRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): BeginRentRequest;
  static deserializeBinaryFromReader(message: BeginRentRequest, reader: jspb.BinaryReader): BeginRentRequest;
}

export namespace BeginRentRequest {
  export type AsObject = {
    registrationnumber: string,
  }
}

export class BeginRentResponse extends jspb.Message {
  getStatus(): BeginRentStatus;
  setStatus(value: BeginRentStatus): BeginRentResponse;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): BeginRentResponse.AsObject;
  static toObject(includeInstance: boolean, msg: BeginRentResponse): BeginRentResponse.AsObject;
  static serializeBinaryToWriter(message: BeginRentResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): BeginRentResponse;
  static deserializeBinaryFromReader(message: BeginRentResponse, reader: jspb.BinaryReader): BeginRentResponse;
}

export namespace BeginRentResponse {
  export type AsObject = {
    status: BeginRentStatus,
  }
}

export class EndRentRequest extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): EndRentRequest.AsObject;
  static toObject(includeInstance: boolean, msg: EndRentRequest): EndRentRequest.AsObject;
  static serializeBinaryToWriter(message: EndRentRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): EndRentRequest;
  static deserializeBinaryFromReader(message: EndRentRequest, reader: jspb.BinaryReader): EndRentRequest;
}

export namespace EndRentRequest {
  export type AsObject = {
  }
}

export class EndRentResponse extends jspb.Message {
  getStatus(): EndRentStatus;
  setStatus(value: EndRentStatus): EndRentResponse;

  getPaidamount(): string;
  setPaidamount(value: string): EndRentResponse;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): EndRentResponse.AsObject;
  static toObject(includeInstance: boolean, msg: EndRentResponse): EndRentResponse.AsObject;
  static serializeBinaryToWriter(message: EndRentResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): EndRentResponse;
  static deserializeBinaryFromReader(message: EndRentResponse, reader: jspb.BinaryReader): EndRentResponse;
}

export namespace EndRentResponse {
  export type AsObject = {
    status: EndRentStatus,
    paidamount: string,
  }
}

export enum BeginRentStatus { 
  SUCCESSFULLY_RENTED = 0,
  VEHICLE_IN_USE = 1,
  VEHICLE_DOESNT_EXIST = 2,
  INSUFFICIENT_FUNDS = 3,
  RENT_IN_PROGRESS = 4,
}
export enum EndRentStatus { 
  SUCCESSFULLY_ENDED_RENT = 0,
  OUTSIDE_AREA = 1,
  NO_VEHICLE_RENTED = 2,
}
