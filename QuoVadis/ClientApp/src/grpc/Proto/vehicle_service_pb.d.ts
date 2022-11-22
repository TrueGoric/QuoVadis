import * as jspb from 'google-protobuf'

import * as Proto_common_pb from '../Proto/common_pb';


export class UpdatePositionRequest extends jspb.Message {
  getVehicleRegistration(): string;
  setVehicleRegistration(value: string): UpdatePositionRequest;

  getLocation(): Proto_common_pb.Location | undefined;
  setLocation(value?: Proto_common_pb.Location): UpdatePositionRequest;
  hasLocation(): boolean;
  clearLocation(): UpdatePositionRequest;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): UpdatePositionRequest.AsObject;
  static toObject(includeInstance: boolean, msg: UpdatePositionRequest): UpdatePositionRequest.AsObject;
  static serializeBinaryToWriter(message: UpdatePositionRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): UpdatePositionRequest;
  static deserializeBinaryFromReader(message: UpdatePositionRequest, reader: jspb.BinaryReader): UpdatePositionRequest;
}

export namespace UpdatePositionRequest {
  export type AsObject = {
    vehicleRegistration: string,
    location?: Proto_common_pb.Location.AsObject,
  }
}

export class UpdatePositionResponse extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): UpdatePositionResponse.AsObject;
  static toObject(includeInstance: boolean, msg: UpdatePositionResponse): UpdatePositionResponse.AsObject;
  static serializeBinaryToWriter(message: UpdatePositionResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): UpdatePositionResponse;
  static deserializeBinaryFromReader(message: UpdatePositionResponse, reader: jspb.BinaryReader): UpdatePositionResponse;
}

export namespace UpdatePositionResponse {
  export type AsObject = {
  }
}

