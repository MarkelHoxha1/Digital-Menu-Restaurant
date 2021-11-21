export interface Dish {
	Id?: string;
	Name?: string;
	Description?: string;
	Price?: number;
	category?: DishCategory;
	IsAvailable?: boolean;
	availability?: Availability[];
	TimeToCook?: number;
}

export enum Availability{
	Breakfast,
	Lunch,
	Dinner,
	WeekDays,
	Weekend,
}

export enum DishCategory
{
	Starter,
	Main_course,
	Dessert,
	Beverage,
	Other,

}

