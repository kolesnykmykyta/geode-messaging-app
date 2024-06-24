import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'countryNumber',
  standalone: true,
})
export class CountryNumberPipe implements PipeTransform {
  transform(value: number, countryCode: string): string {
    let numberAsString = value.toString();
    let [integerPart = '', decimalPart = ''] = numberAsString.split('.');
    let integerTriplets = [];

    for (let i = integerPart.length; i > 0; i -= 3) {
      let start = Math.max(0, i - 3);
      integerTriplets.unshift(integerPart.slice(start, i));
    }

    switch (countryCode) {
      case 'US':
      case 'UK':
      case 'JP':
        return this.joinAllNumberParts(integerTriplets, decimalPart, ',', '.');
      case 'FR':
        return this.joinAllNumberParts(integerTriplets, decimalPart, ' ', ',');
      case 'DE':
        return this.joinAllNumberParts(integerTriplets, decimalPart, '.', ',');
      default:
        return 'ERR_UNKNOWN_COUNTRY_CODE';
    }
  }

  private joinAllNumberParts(
    intTriplets: string[],
    decimalPart: string,
    tripletsDivider: string,
    partsDivider: string
  ) {
    let integerPart = intTriplets.join(tripletsDivider);
    decimalPart = decimalPart ? partsDivider + decimalPart : '';
    return integerPart + decimalPart;
  }
}
