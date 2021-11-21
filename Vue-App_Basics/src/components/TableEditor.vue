<template>
<v-container class="table-editor">
	<v-data-table
		:headers="headers"
		:items="dishes"
		item-key="Id"
		sort-by="calories"
		class="elevation-1"
	>
		<template v-slot:item.availability="{ item }">
			<v-chip 
				v-for="product in getTitle(item.availability)" 
				:key="product"
				outlined
				small
			>{{product}}
			</v-chip>
		</template>
		<template v-slot:item.category="{ item }">
			<v-chip outlined small >{{getCategory(item.category)}}
			</v-chip>
		</template>
		<template v-slot:item.IsAvailable="{ item }">
        <v-switch
			v-model="item.IsAvailable"
			@change="changeState(item)"
			:label="(item.IsAvailable == true ? 'Yes': 'No')"
			></v-switch>
      </template>
		<template v-slot:item.Price="{ item }">
			<p>{{ item.Price }} Euro</p>
		</template>
		<template v-slot:item.TimeToCook="{ item }">
			<p>{{ item.TimeToCook }} minutes</p>
		</template>
		<template v-slot:top>
		<v-toolbar
			flat
		>
			<v-toolbar-title>Dishes</v-toolbar-title>
			<v-divider
			class="mx-4"
			inset
			vertical
			></v-divider>
			<v-spacer></v-spacer>
			<v-dialog
			v-model="dialog"
			max-width="500px"
			>
			<template v-slot:activator="{ on, attrs }">
				<v-btn
				color="primary"
				dark
				class="mb-2"
				v-bind="attrs"
				v-on="on"
				>
				New Dish
				</v-btn>
			</template>
			<v-card>
				<v-card-title>
				<span class="text-h5">{{ formTitle }}</span>
				</v-card-title>

				<v-card-text>
				<v-container>
					<v-row>
					<v-col
						cols="12"
						sm="6"
						md="4"
					>
						<v-text-field
						v-model="editedItem.Name"
						label="Dish name"
						></v-text-field>
					</v-col>
					<v-col
						cols="12"
						sm="6"
						md="4"
					>
						<v-text-field
						v-model="editedItem.Description"
						label="Dish short description"
						></v-text-field>
					</v-col>
					<v-col
						cols="12"
						sm="6"
						md="4"
					>
						<v-text-field
						v-model="editedItem.TimeToCook"
						label="Time to cook (minutes)"
						></v-text-field>
					</v-col>
					<v-col
						cols="12"
						sm="6"
						md="4"
					>
						<v-text-field
						v-model="editedItem.Price"
						label="Price (Euro)"
						></v-text-field>
					</v-col>
					<!-- <v-col
						cols="12"
						sm="6"
						md="4"
					>
						<v-combobox
							v-model="editedItem.category"
							:items="CategoryList"
							label="Category"
							outlined
							dense
						></v-combobox>
					</v-col>
					<v-col
						cols="12"
						sm="6"
						md="4"
					>
					<v-combobox
						v-model="editedItem.availability"
						:items="AvailabilityList"
						label="Availability"
						multiple
						outlined
						dense
					></v-combobox>
					</v-col> -->
					</v-row>
				</v-container>
				</v-card-text>

				<v-card-actions>
				<v-spacer></v-spacer>
				<v-btn
					color="blue darken-1"
					text
					@click="close"
				>
					Cancel
				</v-btn>
				<v-btn
					color="blue darken-1"
					text
					@click="save"
				>
					Save
				</v-btn>
				</v-card-actions>
			</v-card>
			</v-dialog>
			<v-dialog v-model="dialogDelete" max-width="500px">
			<v-card>
				<v-card-title class="text-h5">Are you sure you want to delete this dish?</v-card-title>
				<v-card-actions>
				<v-spacer></v-spacer>
				<v-btn color="blue darken-1" text @click="closeDelete">Cancel</v-btn>
				<v-btn color="blue darken-1" text @click="deleteItemConfirm">OK</v-btn>
				<v-spacer></v-spacer>
				</v-card-actions>
			</v-card>
			</v-dialog>
		</v-toolbar>
		</template>
		<template v-slot:item.actions="{ item }">
		<v-icon
			small
			class="mr-2"
			@click="editItem(item)"
		>
			mdi-pencil
		</v-icon>
		<v-icon
			small
			@click="deleteItem(item)"
		>
			mdi-delete
		</v-icon>
		</template>
		<template v-slot:no-data>
		<v-btn
			color="primary"
			@click="initialize"
		>
			Reset
		</v-btn>
		</template>
	</v-data-table>
</v-container>
</template>

<script lang="ts">
import axios from 'axios';
import { Component, Vue } from 'vue-property-decorator';
import { Prop, Watch } from 'vue-property-decorator';
import { Availability, Dish, DishCategory } from '@/utils/dish';
import { DataTableHeader } from 'vuetify/types';

@Component
export default class TableEditor extends Vue {
	/* ############################## Props ############################## */

	/* ############################## Data ############################### */
	public headers: DataTableHeader[] = [
		{
			text: 'Name',
			value: 'Name',
		},
		{
			text: 'Description',
			value: 'Description',
		},
		{
			text: 'Price',
			value: 'Price',
		},
		{
			text: 'Availability',
			value: 'availability',
		},
		{
			text: 'Category',
			value: 'category',
		},
		{
			text: 'IsAvailable',
			value: 'IsAvailable',
		},
		{
			text: 'TimeToCook',
			value: 'TimeToCook',
		},
		{
			text: 'Actions',
			value: 'actions',
		},
	];
	public search: string = '';
	public dishes: Dish[] = [];
	public dialog: boolean = false;
    public dialogDelete:boolean = false;
	public editedIndex:number = -1;
	public backendUrl:string = "https://localhost:5001/dish";
	public editedItem:Dish =  {
        Name: '',
        Description: "",
        Price: 0,
        category: 0,
        availability: [],
		IsAvailable: true,
		TimeToCook: 0
      };
    public defaultItem:Dish = {
        Name: '',
        Description: "",
        Price: 0,
        category: 0,
        availability: [],
		IsAvailable: true,
		TimeToCook: 0
      };

	public AvailabilityList = Object.keys(Availability).filter((item) => {
		return isNaN(Number(item));
	});

	public CategoryList = Object.keys(DishCategory).filter((item) => {
		return isNaN(Number(item));
	});

	/* ############################## Methods ############################## */

	/**
	 * Calculate the tittle to be shown
	 */
	public getTitle(availability: []): string[] {
		if (availability.length == 0) {return ['']; }
		if(!Array.isArray(availability)){

			availability = availability.split(",");
		}
		var text: string[] = [];
		availability.forEach(element => {
			text.push(Availability[element]);
		});
		
		return text;
	}

		/**
	 * Calculate the tittle to be shown
	 */
	public getCategory(category: number): string {
		return DishCategory[category];
	}

	public async changeState(item: Dish): Promise<void> {
		console.log(item);
		if(item.IsAvailable)
			await axios.post(`${this.backendUrl}/activatedish/${item.Id}`);
		else
			await axios.post(`${this.backendUrl}/deactivatedish/${item.Id}`);
	}

	public async initialize(): Promise<void> {
		const url = this.backendUrl;
		const response = await axios.get(url);
		this.dishes = response.data;
		console.log(this.dishes);
	}

	public async editItem(item: Dish): Promise<void> {
        this.editedIndex = this.dishes.indexOf(item);
        this.editedItem = Object.assign({}, item);
        this.dialog = true;
	}

    public deleteItem(item: Dish): void {
        this.editedIndex = this.dishes.indexOf(item);
        this.editedItem = Object.assign({}, item);
        this.dialogDelete = true;
	}

    public async deleteItemConfirm(): Promise<void> {
        this.dishes.splice(this.editedIndex, 1);
		await axios.delete(`${this.backendUrl}/${this.editedItem.Id}`);
        this.closeDelete();
	}

    public close() {
        this.dialog = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        });
	}

    public closeDelete () {
        this.dialogDelete = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        });
	}

    public async save (): Promise<void> {
        if (this.editedIndex > -1) {
          Object.assign(this.dishes[this.editedIndex], this.editedItem);
		  		await axios.put(`${this.backendUrl}/${this.editedItem.Id}`, {
					  	Id: this.editedItem.Id,
						Name: this.editedItem.Name,
						Description: this.editedItem.Description,
						Price: Number(this.editedItem.Price),
						category: this.editedItem.category,
						Availability: this.editedItem.availability,
						IsAvailable: this.editedItem.IsAvailable,
						TimeToCook : this.editedItem.TimeToCook
					
				  }, {
                    headers: {
                        'Content-Type': 'application/json',
                    }
                }
				  );
        } else {
          this.dishes.push(this.editedItem);
		  	await axios.post(this.backendUrl, {
				Id: this.editedItem.Id,
				Name: this.editedItem.Name,
				Description: this.editedItem.Description,
				Price: Number(this.editedItem.Price),
				category: this.editedItem.category,
				Availability: this.editedItem.availability,
				IsAvailable: this.editedItem.IsAvailable,
				TimeToCook : this.editedItem.TimeToCook
			
			}, {
				headers: {
					'Content-Type': 'application/json',
				}
			}
			  );
		}
        this.close();
	}

	/* ############################## Computed ############################## */
	public get searchValue(): string {
		return  ""; //this.$tsStore.news.searchValue;
	}

	public get formTitle (): string {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
	}

	@Watch('dialog')
	public dialogChanged(val: any): void {
		val || this.close()
	}

	@Watch('dialogDelete')
    public dialogDeleteChanged(val: any): void {
        val || this.closeDelete()
	}

	public created(): void {
		this.initialize();
	}

	public mounted(): void {
	}
}
</script>
<style>
tr.v-data-table__selected {
	-webkit-animation:	argh-my-eyes 2s infinite;
	-moz-animation:		argh-my-eyes 2s infinite;
	animation:			argh-my-eyes 2s infinite;
	animation-iteration-count: 5;
	-moz-animation-iteration-count: 5;
	-webkit-animation-iteration-count: 5;
}
  @-webkit-keyframes argh-my-eyes {
    0%   { background-color: #fff; }
    49% { background-color: #fff; }
    50% { background-color: #64B5F6; }
    99% { background-color: #64B5F6; }
    100% { background-color: #4DB6AC; }
  }
  @-moz-keyframes argh-my-eyes {
    0%   { background-color: #64B5F6; }
    49% { background-color: #64B5F6; }
    50% { background-color: #fff; }
    99% { background-color: #fff; }
    100% { background-color: #64B5F6; }
  }
  @keyframes argh-my-eyes {
    0%   { background-color: #4DB6AC; }
    49% { background-color: #4DB6AC; }
    50% { background-color: #fff; }
    99% { background-color: #fff; }
    100% { background-color: #4DB6AC; }
  }
</style>
<style lang="less" scoped>
.table-editor {
	height: calc(100vh - 180px);
}

.table-editor-overflowed {
	overflow-x: hidden;
	overflow-y: auto;
}

.table-editor-actions {
	@media (min-width: 1300px) {
		min-width: 110px;
	}
	@media (min-width: 1400px) {
		min-width: 180px;
	}
}
</style>