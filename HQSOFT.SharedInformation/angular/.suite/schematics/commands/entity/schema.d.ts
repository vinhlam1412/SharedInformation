import { Template } from '../../enums';
export interface Schema {
    /**
     * A custom selector for entity CRUD component
     */
    selector?: string;
    /**
     * The path to read entity info from
     */
    source: string;
    /**
     * The solution name to generate the code for
     */
    target: string;
    /**
     * Which template to use
     */
    template: Template;
}
