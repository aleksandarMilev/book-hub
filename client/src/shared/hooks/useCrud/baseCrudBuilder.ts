import type { CrudApi } from '@/shared/hooks/useCrud/types/crudApi';
import type { CrudErrors } from '@/shared/hooks/useCrud/types/crudErrors';
import { createCrudHooks } from '@/shared/hooks/useCrud/useCrud';

export default <TAll = null, TDetails = null, TCreate = null>() =>
  new CrudHooksBuilder<TAll, TDetails, TCreate>();

class CrudHooksBuilder<TAll, TDetails, TCreate> {
  private _api!: CrudApi<TAll, TDetails, TCreate>;
  private _resource!: string;
  private _errors!: CrudErrors;

  with() {
    return this;
  }

  and() {
    return this;
  }

  api(api: CrudApi<TAll, TDetails, TCreate>) {
    this._api = api;

    return this;
  }

  resource(name: string) {
    this._resource = name;

    return this;
  }

  errors(errors: CrudErrors) {
    this._errors = errors;

    return this;
  }

  defaultsFor(resource: string, errors: CrudErrors) {
    this._resource = resource;
    this._errors = errors;

    return this;
  }

  create() {
    if (!this._api) {
      throw new Error('API is required. Use .api(...)');
    }

    if (!this._resource) {
      throw new Error('Resource name is required. Use .resource(...)');
    }

    if (!this._errors) {
      throw new Error('Error messages are required. Use .errors(...)');
    }

    return createCrudHooks<TAll, TDetails, TCreate>({
      api: this._api,
      resourceName: this._resource,
      errors: this._errors,
    });
  }
}
