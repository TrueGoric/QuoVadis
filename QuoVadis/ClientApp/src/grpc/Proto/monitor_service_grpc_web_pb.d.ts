import * as grpcWeb from 'grpc-web';

import * as Proto_monitor_service_pb from '../Proto/monitor_service_pb';


export class MonitorClient {
  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: any; });

  monitorLocation(
    request: Proto_monitor_service_pb.MonitorLocationRequest,
    metadata?: grpcWeb.Metadata
  ): grpcWeb.ClientReadableStream<Proto_monitor_service_pb.MonitorLocationEvent>;

}

export class MonitorPromiseClient {
  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: any; });

  monitorLocation(
    request: Proto_monitor_service_pb.MonitorLocationRequest,
    metadata?: grpcWeb.Metadata
  ): grpcWeb.ClientReadableStream<Proto_monitor_service_pb.MonitorLocationEvent>;

}

