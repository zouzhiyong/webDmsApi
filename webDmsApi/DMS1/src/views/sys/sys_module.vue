<template> 
<layout>
  <moduleTree slot="tree"></moduleTree>   
  <moduleTable slot="table" ref="table"></moduleTable> 
  <moduleForm slot="form" ref="form"></moduleForm>
</layout>
</template>

<script>
import layout from "./../layout/layout_left_right";
import moduleTree from "./sys_module_tree";
import moduleTable from "./sys_module_table";
import moduleForm from "./sys_module_form";

export default {
  components: {
    layout,
    moduleForm,
    moduleTree,
    moduleTable
  }
};
</script>

