export interface IFilter {
  searchParam: string | null;
  sortProp: string | null;
  sortByDescending: boolean | null;
  selectProps: string | null;
  pageNumber: number | null;
}
