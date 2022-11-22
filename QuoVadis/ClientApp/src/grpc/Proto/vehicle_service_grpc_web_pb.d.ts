import * as grpcWeb from 'grpc-web';

import * as Proto_vehicle_service_pb from '../Proto/vehicle_service_pb';


export class VehicleClient {
  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: any; });

  updatePosition(
    request: Proto_vehicle_service_pb.UpdatePositionRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_vehicle_service_pb.UpdatePositionResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_vehicle_service_pb.UpdatePositionResponse>;

}

export class VehiclePromiseClient {
  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: any; });

  updatePosition(
    request: Proto_vehicle_service_pb.UpdatePositionRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_vehicle_service_pb.UpdatePositionResponse>;

}

