import { Injectable } from '@angular/core';
import { IFilter } from '../models/filter.model';

@Injectable({
  providedIn: 'root',
})
export class RequestsHelperService {
  constructor() {}

  generateQueryParamsByFilter(filter: IFilter): string {
    return (Object.keys(filter) as Array<keyof IFilter>)
      .filter((key) => !!filter[key])
      .map(
        (key) =>
          `${encodeURIComponent(key)}=${encodeURIComponent(
            filter[key] as string
          )}`
      )
      .join('&');
  }
}
