import * as jspb from 'google-protobuf'

import * as Proto_common_pb from '../Proto/common_pb';


export class MonitorLocationRequest extends jspb.Message {
  getArea(): string;
  setArea(value: string): MonitorLocationRequest;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): MonitorLocationRequest.AsObject;
  static toObject(includeInstance: boolean, msg: MonitorLocationRequest): MonitorLocationRequest.AsObject;
  static serializeBinaryToWriter(message: MonitorLocationRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): MonitorLocationRequest;
  static deserializeBinaryFromReader(message: MonitorLocationRequest, reader: jspb.BinaryReader): MonitorLocationRequest;
}

export namespace MonitorLocationRequest {
  export type AsObject = {
    area: string,
  }
}

export class MonitorLocationEvent extends jspb.Message {
  getRegistrationnumber(): string;
  setRegistrationnumber(value: string): MonitorLocationEvent;

  getLocation(): Proto_common_pb.Location | undefined;
  setLocation(value?: Proto_common_pb.Location): MonitorLocationEvent;
  hasLocation(): boolean;
  clearLocation(): MonitorLocationEvent;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): MonitorLocationEvent.AsObject;
  static toObject(includeInstance: boolean, msg: MonitorLocationEvent): MonitorLocationEvent.AsObject;
  static serializeBinaryToWriter(message: MonitorLocationEvent, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): MonitorLocationEvent;
  static deserializeBinaryFromReader(message: MonitorLocationEvent, reader: jspb.BinaryReader): MonitorLocationEvent;
}

export namespace MonitorLocationEvent {
  export type AsObject = {
    registrationnumber: string,
    location?: Proto_common_pb.Location.AsObject,
  }
}

