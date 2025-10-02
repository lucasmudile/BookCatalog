import { PagedParameters } from '../pagination/paged-parameters.interface';

export interface SearchParameters extends PagedParameters {
  name?: string;
  query?: string;
}
