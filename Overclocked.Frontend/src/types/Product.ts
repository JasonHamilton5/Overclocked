export interface Product
{
  productId: number;
  name: string;
  sku: string;
  price: number;
  qty: number;
  image: string | null;
  category: string | null;
  isFeatured: boolean;
}