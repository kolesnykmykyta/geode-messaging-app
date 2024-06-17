export function partialize<T extends object>(obj: T): Partial<T> {
  return Object.keys(obj)
    .filter((key) => obj[key as keyof T] || obj[key as keyof T] === 0)
    .reduce((result, key) => {
      result[key as keyof T] = obj[key as keyof T];
      return result;
    }, {} as Partial<T>);
}
