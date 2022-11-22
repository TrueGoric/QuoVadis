import * as grpcWeb from 'grpc-web';

import * as Proto_user_service_pb from '../Proto/user_service_pb';


export class UserClient {
  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: any; });

  register(
    request: Proto_user_service_pb.RegisterRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_user_service_pb.RegisterResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_user_service_pb.RegisterResponse>;

  login(
    request: Proto_user_service_pb.LoginRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_user_service_pb.LoginResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_user_service_pb.LoginResponse>;

  checkBalance(
    request: Proto_user_service_pb.CheckBalanceRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_user_service_pb.CheckBalanceResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_user_service_pb.CheckBalanceResponse>;

  addFunds(
    request: Proto_user_service_pb.AddFundsRequest,
    metadata: grpcWeb.Metadata | undefined,
    callback: (err: grpcWeb.RpcError,
               response: Proto_user_service_pb.AddFundsResponse) => void
  ): grpcWeb.ClientReadableStream<Proto_user_service_pb.AddFundsResponse>;

}

export class UserPromiseClient {
  constructor (hostname: string,
               credentials?: null | { [index: string]: string; },
               options?: null | { [index: string]: any; });

  register(
    request: Proto_user_service_pb.RegisterRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_user_service_pb.RegisterResponse>;

  login(
    request: Proto_user_service_pb.LoginRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_user_service_pb.LoginResponse>;

  checkBalance(
    request: Proto_user_service_pb.CheckBalanceRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_user_service_pb.CheckBalanceResponse>;

  addFunds(
    request: Proto_user_service_pb.AddFundsRequest,
    metadata?: grpcWeb.Metadata
  ): Promise<Proto_user_service_pb.AddFundsResponse>;

}

