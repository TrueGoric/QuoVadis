import * as grpcWeb from 'grpc-web';

import * as Proto_rent_service_pb from '../Proto/rent_service_pb';


export class RentClient {
  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: any; });

  getAreas(
    request: Proto_rent_service_pb.GetAreasRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_rent_service_pb.GetAreasResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_rent_service_pb.GetAreasResponse>;

  getVehicles(
    request: Proto_rent_service_pb.GetVehiclesRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_rent_service_pb.GetVehiclesResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_rent_service_pb.GetVehiclesResponse>;

  getCurrentlyRentedVehicle(
    request: Proto_rent_service_pb.GetCurrentlyRentedVehicleRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_rent_service_pb.GetCurrentlyRentedVehicleResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_rent_service_pb.GetCurrentlyRentedVehicleResponse>;

  beginRent(
    request: Proto_rent_service_pb.BeginRentRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_rent_service_pb.BeginRentResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_rent_service_pb.BeginRentResponse>;

  endRent(
    request: Proto_rent_service_pb.EndRentRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_rent_service_pb.EndRentResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_rent_service_pb.EndRentResponse>;

}

export class RentPromiseClient {
  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: any; });

  getAreas(
    request: Proto_rent_service_pb.GetAreasRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_rent_service_pb.GetAreasResponse>;

  getVehicles(
    request: Proto_rent_service_pb.GetVehiclesRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_rent_service_pb.GetVehiclesResponse>;

  getCurrentlyRentedVehicle(
    request: Proto_rent_service_pb.GetCurrentlyRentedVehicleRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_rent_service_pb.GetCurrentlyRentedVehicleResponse>;

  beginRent(
    request: Proto_rent_service_pb.BeginRentRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_rent_service_pb.BeginRentResponse>;

  endRent(
    request: Proto_rent_service_pb.EndRentRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_rent_service_pb.EndRentResponse>;

}

