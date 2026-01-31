import { HttpStatusCode } from 'axios';

export class HttpError extends Error {
  status: number;

  private constructor(message: string, name: string, status: number) {
    super(message);
    this.name = name;
    this.status = status;

    Object.setPrototypeOf(this, HttpError.prototype);
  }

  static with() {
    class Builder {
      private _message = 'Something went wrong!';
      private _name = 'Http Error';
      private _status = HttpStatusCode.BadRequest;

      with() {
        return this;
      }

      and() {
        return this;
      }

      message(message: string) {
        this._message = message;
        return this;
      }

      name(name: string) {
        this._name = name;
        return this;
      }

      status(status: number) {
        this._status = status;
        return this;
      }

      create() {
        return new HttpError(this._message, this._name, this._status);
      }
    }

    return new Builder();
  }
}

