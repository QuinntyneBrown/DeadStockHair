export interface Retailer {
  id: string;
  name: string;
  url: string;
  status: RetailerStatus;
  discoveredAt: string;
  lastCheckedAt: string | null;
}

export enum RetailerStatus {
  InStock = 0,
  OutOfStock = 1,
  Unknown = 2,
}

export interface RetailerStats {
  totalRetailers: number;
  inStock: number;
  newThisWeek: number;
}
